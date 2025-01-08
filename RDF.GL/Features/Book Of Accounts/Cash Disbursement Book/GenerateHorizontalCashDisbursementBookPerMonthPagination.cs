using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Cash_Disbursement_Book;
[Route("api/book-of-accounts"), ApiController]


//Class for the whole Cash Disbursement Book
public class GenerateHorizontalCashDisbursementBookPerMonthPagination(IMediator mediator) : ControllerBase
{
    //Controller for the whole Cash Disbursement Book
    [HttpGet("cash-disbursement-book/horizontal/page")]
    public async Task<IActionResult> GetCashDisbursementBookPerMonth(
        [FromQuery] GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand request)
    {
        try
        {
            var cashDisbursementBook = await mediator.Send(request);
            Response.AddPaginationHeader(
                cashDisbursementBook.CurrentPage,
                cashDisbursementBook.PageSize,
                cashDisbursementBook.TotalCount,
                cashDisbursementBook.TotalPages,
                cashDisbursementBook.HasPreviousPage,
                cashDisbursementBook.HasNextPage
                );

            var result = new
            {
                cashDisbursementBook,
                cashDisbursementBook.CurrentPage,
                cashDisbursementBook.PageSize,
                cashDisbursementBook.TotalCount,
                cashDisbursementBook.TotalPages,
                cashDisbursementBook.HasPreviousPage,
                cashDisbursementBook.HasNextPage
            };
            
            var successResult = Result.Success(result);
            
            return Ok(successResult);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Command for Cash Disbursement Book
    public class GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand : UserParams, IRequest<PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>>
    {
        public string Search { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
    }


    //Response for Cash Disbursement Book
    public record GenerateHorizontalCashDisbursementBookPerMonthResponse
    {
        public string ChequeDate { get; set; }
        public string Bank { get; set; }
        public string CvNumber { get; set; }
        public string ChequeNumber { get; set; }
        public string Payee { get; set; }
        public string Description  { get; set; }
        public string TagNumber { get; set; }
        public string ApvNumber { get; set; }
        public IEnumerable<ChartOfAccount> AccountName { get; set; }

        public class ChartOfAccount
        {
            public string NameOfAccount { get; set; }
            public decimal? Debit { get; set; }
            public decimal? Credit { get; set; }
            public string DrCr { get; set; }
        }
    }

    //Handler for Cash Disbursement Book

    public class Handler(ProjectGLDbContext context)
        : IRequestHandler<GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand, PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>>
    {

        //Method for Cash Disbursement Book
        public async Task<PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>> Handle(GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand request,
            CancellationToken cancellationToken)
        {
            var cashDisbursementBook = context.GeneralLedgers
                .Where(gl =>  string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                             && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0)
                .Where(gl => gl.BOA == "Cash Disbursement Book")
                .GroupBy(gl => new
                {
                    gl.Month,
                    gl.Year,
                    gl.BankName,
                    gl.ChequeVoucherNumber,
                    gl.ChequeNumber
                });
            
            var result = cashDisbursementBook
                .Select(sj => new GenerateHorizontalCashDisbursementBookPerMonthResponse
                {
                    ChequeDate = $"{sj.Key.Month}, {sj.Key.Year}",
                    Bank = sj.Key.BankName,
                    CvNumber = sj.Key.ChequeVoucherNumber,
                    ChequeNumber = sj.Key.ChequeNumber,
                    Payee = sj.First().Particulars,
                    Description = sj.First().ItemDescription,
                    TagNumber = sj.First().Asset,
                    ApvNumber = null,
                    AccountName = sj
                        .GroupBy(x => x.AccountTitle)
                        .Select(accountGroup =>
                            new GenerateHorizontalCashDisbursementBookPerMonthResponse.ChartOfAccount
                            {
                                NameOfAccount = accountGroup.Key,
                                Credit = accountGroup.Sum(g => g.LineAmount < 0 ? g.LineAmount : 0),
                                Debit = accountGroup.Sum(g => g.LineAmount > 0 ? g.LineAmount : 0),
                                DrCr = accountGroup.First().DRCP
                            }).ToList()
                });
            
            return await PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>.CreateAsync(result, request.PageNumber, request.PageSize);

        }
    }
}