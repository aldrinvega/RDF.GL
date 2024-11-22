using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Cash_Disbursement_Book;
[Route("api/book-of-accounts"), ApiController]

public class GenerateHorizontalCashDisbursementBookPerMonthPagination(IMediator mediator) : ControllerBase
{

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
    public class GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand : UserParams, IRequest<PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>>
    {
        public string Search { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }

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

    public class Handler(ProjectGLDbContext context)
        : IRequestHandler<GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand, PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>>
    {
        public async Task<PagedList<GenerateHorizontalCashDisbursementBookPerMonthResponse>> Handle(GenerateHorizontalCashDisbursementBookPerMonthPaginationCommand request,
            CancellationToken cancellationToken)
        {
            var cashDisbursementBook = context.GeneralLedgers
                .Where(gl => gl.Month == request.Month)
                .Where(gl => gl.Year == request.Year)
                .Where(gl => gl.System == Common.System.Fisto)
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