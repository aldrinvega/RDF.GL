using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Cash_Disbursement_Book;
[Route("api/book-of-accounts"), ApiController()]

public class GenerateVerticalCashDisbursementBookPerMonth(IMediator mediator) : ControllerBase
{
    [HttpGet("cash-disbursement-book/vertical")]
    public async Task<IActionResult> GetCashDisbursementBookPerMonth([FromQuery] GenerateCashDisbursementBookPerMonthCommand request)
    {
        try
        {
            var result = await mediator.Send(request);
            return result.IsFailure ? BadRequest(result) : Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class GenerateCashDisbursementBookPerMonthCommand : IRequest<Result>
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
    }

    public class GenerateCashDisbursementBookPerMonthResponse
    {
        public string ChequeDate { get; set; }
        public string Bank { get; set; }
        public string CvNumber { get; set; }
        public string ChequeNumber { get; set; }
        public string Payee { get; set; }
        public string Description  { get; set; }
        public string TagNumber { get; set; }
        public string ApvNumber { get; set; }
        public string AccountName { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public string DrCr { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateCashDisbursementBookPerMonthCommand, Result>
    {
        public async Task<Result> Handle(GenerateCashDisbursementBookPerMonthCommand request,
            CancellationToken cancellationToken)
        {
            var glReports = await context.GeneralLedgers.Where(gl =>
                    gl.Month == request.Month && 
                    gl.Year == request.Year &&
                    gl.BOA == "Cash Disbursement Book")
                .ToListAsync(cancellationToken: cancellationToken);

            var cashDisbursementBook = glReports.Select(cdb => new GenerateCashDisbursementBookPerMonthResponse
            {
                ChequeDate = $"{cdb.Month}, {cdb.Year}",
                Bank = cdb.BankName,
                CvNumber = cdb.ChequeVoucherNumber,
                ChequeNumber = cdb.ChequeNumber,
                Payee = cdb.Particulars,
                Description = cdb.ItemDescription,
                TagNumber = cdb.Asset,
                ApvNumber = cdb.ChequeVoucherNumber,
                AccountName = cdb.AccountTitle,
                CreditAmount = cdb.LineAmount < 0 ? cdb.LineAmount : 0,
                DebitAmount = cdb.LineAmount > 0 ? cdb.LineAmount : 0,
                DrCr = cdb.DRCP
            });

            return Result.Success(cashDisbursementBook);
        }
    }
}