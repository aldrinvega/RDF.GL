using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Book_Of_Accounts.Purchases_Book;
[Route("api/book-of-accounts"), ApiController]

public class GenerateHorizontalPurchasesBookPerMonthPagination(IMediator mediator) : ControllerBase
{
    [HttpGet("purchases-book/horizontal/page")]
    public async Task<IActionResult> Get([FromQuery] GenerateHorizontalPurchasesBookPerMonthCommand request)
    {
        try
        {
            var purchasesBook = await mediator.Send(request);
            Response.AddPaginationHeader(
                purchasesBook.CurrentPage, 
                purchasesBook.PageSize, 
                purchasesBook.TotalCount, 
                purchasesBook.TotalPages,
                purchasesBook.HasPreviousPage,
                purchasesBook.HasNextPage
                );
            var result = new
            {
                purchasesBook,
                purchasesBook.CurrentPage,
                purchasesBook.PageSize,
                purchasesBook.TotalCount,
                purchasesBook.TotalPages,
                purchasesBook.HasPreviousPage,
                purchasesBook.HasNextPage
            };

            var successResult = Result.Success(result);
            return Ok(successResult);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class GenerateHorizontalPurchasesBookPerMonthCommand : UserParams, IRequest<PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>>
    {
        public string Month { get; set; }
        public string Year { get; set; }
    }
    
    public class GenerateHorizontalPurchasesBookPerMonthResponse
    {
        public string GlDate { get; set; }
        public string TransactionDate { get; set; }
        public string NameOfSupplier { get; set; }
        public string Description { get; set; }
        public string POoNumber { get; set; }
        public string RrNumber { get; set; }
        public string Apv { get; set; }
        public string ReceiptNumber { get; set; }
        public string Amount { get; set; }
        public IEnumerable<ChartOfAccount> AccountName { get; set; }

        public class ChartOfAccount
        {
            public string NameOfAccount { get; set; }
            public decimal? Debit { get; set; }
            public decimal? Credit { get; set; }
            public string DrCr { get; set; }
        }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateHorizontalPurchasesBookPerMonthCommand, PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>>
    {
        public async Task<PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>> Handle(
            GenerateHorizontalPurchasesBookPerMonthCommand request, CancellationToken cancellationToken)
        {
            var purchasesBook = context.GeneralLedgers
                .Where(gl => gl.Month == request.Month)
                .Where(gl => gl.Year == request.Year)
                .Where(gl => gl.System == Common.System.Fisto || 
                             gl.System == Common.System.Ymir || 
                             gl.System == Common.System.Stalwart ||
                             gl.System == Common.System.Manual ||
                             gl.BOA == "Purchases Book") 
                .GroupBy(gl => new
                {
                    gl.Month,
                    gl.Year,
                    gl.ClientSupplier,
                    gl.PONumber,
                    gl.RRNumber,
                    gl.ReferenceNo
                });

            var result = purchasesBook
                .Select(sj => new GenerateHorizontalPurchasesBookPerMonthResponse
                {
                    GlDate = $"{sj.Key.Month}, {sj.Key.Year}",
                    TransactionDate = sj.First().TransactionDate.ToString("MM/dd/yyyy"),
                    NameOfSupplier = sj.Key.ClientSupplier,
                    Description = sj.First().ItemDescription,
                    POoNumber = sj.First().PONumber,
                    RrNumber = sj.First().RRNumber,
                    Apv = sj.First().ChequeVoucherNumber,
                    ReceiptNumber = sj.First().ReferenceNo,
                    Amount = sj.First().LineAmount.ToString(),
                    AccountName = sj
                        .GroupBy(x => x.AccountTitle)
                        .Select(accountGroup =>
                            new GenerateHorizontalPurchasesBookPerMonthResponse.ChartOfAccount
                            {
                                NameOfAccount = accountGroup.Key,
                                Credit = accountGroup.Sum(g => g.LineAmount < 0 ? g.LineAmount : 0),
                                Debit = accountGroup.Sum(g => g.LineAmount > 0 ? g.LineAmount : 0),
                                DrCr = accountGroup.First().DRCP
                            }).ToList()
                });

            return await PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>.CreateAsync(result,
                request.PageNumber, request.PageSize);
        }
    }
}