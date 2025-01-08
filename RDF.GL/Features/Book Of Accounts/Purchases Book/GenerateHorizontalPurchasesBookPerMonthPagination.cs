using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Purchases_Book;
[Route("api/book-of-accounts"), ApiController]

public class GenerateHorizontalPurchasesBookPerMonthPagination(IMediator mediator) : ControllerBase
{

    //Contrller for purchases book horizontal pagination
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

    //Horizontal puchases book command
    public class GenerateHorizontalPurchasesBookPerMonthCommand : UserParams, IRequest<PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>>
    {
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
    }
    
    //Horizontal purchases book response
    public class GenerateHorizontalPurchasesBookPerMonthResponse
    {
        public string GlDate { get; set; }
        public string TransactionDate { get; set; }
        public string NameOfSupplier { get; set; }
        public string Description { get; set; }
        public string PONumber { get; set; }
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
    

    //Horizontal purchases book handler
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateHorizontalPurchasesBookPerMonthCommand, PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>>
    {
        public async Task<PagedList<GenerateHorizontalPurchasesBookPerMonthResponse>> Handle(
            GenerateHorizontalPurchasesBookPerMonthCommand request, CancellationToken cancellationToken)
        {
            var purchasesBook = context.GeneralLedgers
                 .Where(gl => gl.System == Common.System.Ymir ||
                                 gl.System == Common.System.Stalwart ||
                                 gl.System == Common.System.Fisto ||
                                 gl.System == Common.System.Manual &&
                                 (gl.BOA == "Purchases Book"
                                    && string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                                    && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0))
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
                    PONumber = sj.First().PONumber,
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