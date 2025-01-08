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

    //Contrller for purchases book pagination
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

    //Purchases book pagination command
    public class GenerateVerticalPurchasesBookPerMonthPaginationCommand : UserParams,
        IRequest<PagedList<GeneratePurchasesBookPerMonthPaginationResponse>>
    {
        public string Search { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
    }

    //Purchases book pagination response

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


    //Purchases book handler
    public class Handler(ProjectGLDbContext context)
        : IRequestHandler<GenerateVerticalPurchasesBookPerMonthPaginationCommand,
            PagedList<GeneratePurchasesBookPerMonthPaginationResponse>>
    {
        public async Task<PagedList<GeneratePurchasesBookPerMonthPaginationResponse>> Handle(
            GenerateVerticalPurchasesBookPerMonthPaginationCommand request, CancellationToken cancellationToken)
        {
            var purchasesBook = context.GeneralLedgers
                .Where(gl => gl.System == Common.System.Ymir ||
                                 gl.System == Common.System.Stalwart ||
                                 gl.System == Common.System.Fisto ||
                                 gl.System == Common.System.Manual &&
                                 gl.BOA == "Purchases Book"
                                    && string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                                    && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0);


            if (!string.IsNullOrEmpty(request.Search))
            {
                purchasesBook = purchasesBook.Where(gl => gl.ItemDescription.Contains(request.Search));
            }

            var result = purchasesBook.Select(pb => new GeneratePurchasesBookPerMonthPaginationResponse
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
                Amount = pb.LineAmount,
                NameOfAccount = pb.AccountTitle,
                DrCr = pb.DRCP
            });

            return await PagedList<GeneratePurchasesBookPerMonthPaginationResponse>.CreateAsync(result,
                request.PageNumber, request.PageSize);
        }
    }
}