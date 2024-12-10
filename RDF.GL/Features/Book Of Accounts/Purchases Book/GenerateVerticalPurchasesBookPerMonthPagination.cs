using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Book_Of_Accounts.Purchases_Book;
[Route("api/book-of-accounts"), ApiController]

public class GenerateVerticalPurchasesBookPerMonthPagination(IMediator mediator) : ControllerBase
{
    [HttpGet("purchases-book/page")]
    public async Task<IActionResult> Get([FromQuery] GenerateVerticalPurchasesBookPerMonthPaginationCommand request)
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class GenerateVerticalPurchasesBookPerMonthPaginationCommand : UserParams,
        IRequest<PagedList<GeneratePurchasesBookPerMonthPaginationResponse>>
    {
        public string Search { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Year { get; set; }
    }

    public record GeneratePurchasesBookPerMonthPaginationResponse
    {
        public string GlDate { get; set; }
        public string TransactionDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string NameOfSupplier { get; set; }
        public string Description { get; set; }
        public string PoNumber { get; set; }
        public string RrNumber { get; set; }
        public string Apv { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal? Amount { get; set; }
        public string NameOfAccount { get; set; }
        public string DrCr { get; set; }
    }

    public class Handler(ProjectGLDbContext context)
        : IRequestHandler<GenerateVerticalPurchasesBookPerMonthPaginationCommand,
            PagedList<GeneratePurchasesBookPerMonthPaginationResponse>>
    {
        public async Task<PagedList<GeneratePurchasesBookPerMonthPaginationResponse>> Handle(
            GenerateVerticalPurchasesBookPerMonthPaginationCommand request, CancellationToken cancellationToken)
        {
            IQueryable<GeneralLedger> purchasesBook = context.GeneralLedgers
                .Where(gl => gl.Year == request.Year)
                .Where(gl => gl.System == Common.System.Ymir ||
                             gl.System == Common.System.Stalwart ||
                             gl.System == Common.System.Fisto ||
                             gl.System == Common.System.Manual &&
                             gl.BOA == "Purchases Book");

            if (!string.IsNullOrEmpty(request.Search))
            {
                purchasesBook = purchasesBook.Where(gl => gl.ItemDescription.Contains(request.Search));
            }

            var monthRangeQuery = purchasesBook
                .Where(gl =>
                        (gl.Month == "JAN" ? 1 :
                        gl.Month == "FEB" ? 2 :
                        gl.Month == "MAR" ? 3 :
                        gl.Month == "APR" ? 4 :
                        gl.Month == "MAY" ? 5 :
                        gl.Month == "JUN" ? 6 :
                        gl.Month == "JUL" ? 7 :
                        gl.Month == "AUG" ? 8 :
                        gl.Month == "SEP" ? 9 :
                        gl.Month == "OCT" ? 10 :
                        gl.Month == "NOV" ? 11 :
                        gl.Month == "DEC" ? 12 : 0) >= request.From &&
                        (gl.Month == "JAN" ? 1 :
                        gl.Month == "FEB" ? 2 :
                        gl.Month == "MAR" ? 3 :
                        gl.Month == "APR" ? 4 :
                        gl.Month == "MAY" ? 5 :
                        gl.Month == "JUN" ? 6 :
                        gl.Month == "JUL" ? 7 :
                        gl.Month == "AUG" ? 8 :
                        gl.Month == "SEP" ? 9 :
                        gl.Month == "OCT" ? 10 :
                        gl.Month == "NOV" ? 11 :
                        gl.Month == "DEC" ? 12 : 0) <= request.To);

            var result = monthRangeQuery.Select(pb => new GeneratePurchasesBookPerMonthPaginationResponse
            {
                GlDate = $"{pb.Month}. {pb.Year}",
                Month = pb.Month,
                Year = pb.Year,
                TransactionDate = pb.TransactionDate.ToString("yyyy-MM-dd"),
                NameOfSupplier = pb.ClientSupplier,
                Description = pb.ItemDescription,
                Apv = pb.VoucherJournal,
                PoNumber = pb.PONumber,
                RrNumber = pb.RRNumber,
                ReceiptNumber = pb.ReferenceNo,
                Amount = pb.LineAmount > 0 ? pb.LineAmount.Value : 0,
                NameOfAccount = pb.AccountTitle,
                DrCr = pb.DRCP
            });

            return await PagedList<GeneratePurchasesBookPerMonthPaginationResponse>.CreateAsync(result,
                request.PageNumber, request.PageSize);
        }

    }
}