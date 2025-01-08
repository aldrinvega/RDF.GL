using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Trial_Balance;

[Route("api/bookofaccounts"), ApiController]
public class GenerateTrialBalancePerMonth : ControllerBase
{
    private readonly IMediator _mediator;

    public GenerateTrialBalancePerMonth(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("trialbalance")]
    public async Task<IActionResult> Get([FromQuery] GenerateTrialBalancePerMonthCommand request)
    {
        try
        {
            var trialBalance = await _mediator.Send(request);
            return trialBalance.IsFailure ? BadRequest(trialBalance) : Ok(trialBalance);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class GenerateTrialBalancePerMonthCommand : IRequest<Result>
    {
        public string Search { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string FromYear { get; set; }
    }
    
    public record GenerateTrialBalancePerMonthResponse
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? Amount { get; set; }
        public string ChartOfAccount { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string Drcr { get; set; }
        public string Month { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateTrialBalancePerMonthCommand, Result>
    {
        public async Task<Result> Handle(GenerateTrialBalancePerMonthCommand request,
            CancellationToken cancellationToken)
        {

            var trialBalance = await context.GeneralLedgers
                .Where(gl => string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                             && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0)
                .GroupBy(x => new
                {
                    x.AccountTitle
                })
                .Select(gl => new GenerateTrialBalancePerMonthResponse
                {
                    Month = gl.First().Month,
                    Date = gl.First().TransactionDate.ToString("MM/dd/yyyy"),
                    CustomerName = gl.First().ClientSupplier,
                    ReferenceNumber = gl.First().ReferenceNo,
                    Amount = gl.Sum(tb => tb.LineAmount > 0 ? tb.LineAmount : 0),
                    ChartOfAccount = gl.Key.AccountTitle,
                    Debit = gl.Sum(tb => tb.LineAmount > 0 ? tb.LineAmount : 0),
                    Credit = gl.Sum(tb => tb.LineAmount < 0 ? tb.LineAmount : 0),
                    Drcr = gl.First().DRCP
                })
                .ToListAsync(cancellationToken: cancellationToken);

            return Result.Success(trialBalance);
        }
    }
}