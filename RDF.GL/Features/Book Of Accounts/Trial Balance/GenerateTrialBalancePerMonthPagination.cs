using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Book_Of_Accounts.Trial_Balance;

[Route("api/bookofaccounts"), ApiController]

public class GenerateTrialBalancePerMonthPagination(IMediator mediator) : ControllerBase
{
    [HttpGet("trialbalance/page")]
    public async Task<IActionResult> Get([FromQuery] GenerateTrialBalancePerMonthPaginationCommand request)
    {
        try
        {
            var trialBalance = await mediator.Send(request);

            Response.AddPaginationHeader(
                trialBalance.CurrentPage, 
                trialBalance.PageSize, 
                trialBalance.TotalCount, 
                trialBalance.TotalPages,
                trialBalance.HasPreviousPage,
                trialBalance.HasNextPage
            );

            var result = new
            {
                trialBalance,
                trialBalance.CurrentPage,
                trialBalance.PageSize,
                trialBalance.TotalCount,
                trialBalance.TotalPages,
                trialBalance.HasPreviousPage,
                trialBalance.HasNextPage
            };
            var successResult = Result.Success(result);
            return Ok(successResult);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class GenerateTrialBalancePerMonthPaginationCommand : UserParams,
        IRequest<PagedList<GenerateTrialBalancePerMonthPaginationResponse>>
    {
        public string Search { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
    }
    
    public class GenerateTrialBalancePerMonthPaginationResponse
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? Amount { get; set; }
        public string ChartOfAccount { get; set; }
        public decimal? Decimal { get; set; }
        public decimal? Credit { get; set; }
        public string DrCr { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateTrialBalancePerMonthPaginationCommand, PagedList<GenerateTrialBalancePerMonthPaginationResponse>>
    {
        public async Task<PagedList<GenerateTrialBalancePerMonthPaginationResponse>> Handle(
            GenerateTrialBalancePerMonthPaginationCommand request, CancellationToken cancellationToken)
        {
            IQueryable<GeneralLedger> trialBalance = context.GeneralLedgers
                .Where(gl => gl.Month == request.Month)
                .Where(gl => gl.Year == request.Year)
                .Where(gl => gl.System == request.System);
            
            var result = trialBalance
                .Select(tb => new GenerateTrialBalancePerMonthPaginationResponse
                {
                    Date = tb.TransactionDate.ToString("MM/dd/yyyy"),
                    CustomerName = tb.ClientSupplier,
                    ReferenceNumber = tb.ReferenceNo,
                    Amount = tb.LineAmount < 0 ? tb.LineAmount : 0,
                    ChartOfAccount = tb.AccountTitle,
                    Decimal = tb.LineAmount < 0 ? tb.LineAmount : 0,
                    Credit = tb.LineAmount > 0 ? tb.LineAmount : 0,
                    DrCr = tb.DRCP
                });
            
            return await PagedList<GenerateTrialBalancePerMonthPaginationResponse>.CreateAsync(result, request.PageNumber, request.PageSize);
        }
    }
}