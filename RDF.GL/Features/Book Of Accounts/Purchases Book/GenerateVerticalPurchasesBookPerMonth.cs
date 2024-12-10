using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Purchases_Book;
[Route("api/book-of-accounts"), ApiController]

public class GenerateVerticalPurchasesBookPerMonth(IMediator mediator) : ControllerBase
{
    [HttpGet("purchases-book")]
    public async Task<IActionResult> GetPurchaseBookPerMonth([FromQuery] GeneratePurchasesBookPerMonthCommand request)
    {
        try
        {
            var result = await mediator.Send(request);
            return result.IsFailure ? BadRequest(result) : Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    public class GeneratePurchasesBookPerMonthCommand : IRequest<Result>
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Year { get; set; }
    }

    public class GeneratePurchasesBookPerMonthResponse
    {
        public string GlDate  { get; set; }
        public string TransactionDate { get; set; }
        public string NameOfSupplier  { get; set; }
        public string Description { get; set; }
        public string PoNumber { get; set; }
        public string RrNumber { get; set; }
        public string Apv { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal? Amount  { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string NameOfAccount { get; set; }
        public string DRCR { get; set; }
        
    }
    
    public class Handler(ProjectGLDbContext  context) : IRequestHandler<GeneratePurchasesBookPerMonthCommand, Result>
    {
        public async Task<Result> Handle(GeneratePurchasesBookPerMonthCommand request, CancellationToken cancellationToken)
        {
            var monthMapping = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "JAN", 01 }, { "FEB", 02 }, { "MAR", 03 }, { "APR", 04 },
                { "MAY", 05 }, { "JUN", 06 }, { "JUL", 07 }, { "AUG", 08 },
                { "SEP", 09 }, { "OCT", 10 }, { "NOV", 11 }, { "DEC", 12 }
            };
            
            var purchasesBook = (await context.GeneralLedgers
                    .Where(gl => gl.Year == request.Year)
                    .Where(gl => gl.System == Common.System.Ymir || 
                                 gl.System == Common.System.Stalwart || 
                                 gl.System == Common.System.Fisto ||    
                                 gl.System == Common.System.Manual &&
                                 gl.BOA == "Purchases Book")
                    .ToListAsync(cancellationToken))
                .Where(gl => monthMapping[gl.Month] >= request.From && monthMapping[gl.Month] <= request.To)
                .ToList();

            var result = purchasesBook.Select(pb => new GeneratePurchasesBookPerMonthResponse
            {
                GlDate = $"{pb.Month}. {pb.Year}",
                TransactionDate = pb.TransactionDate.ToString("yyyy-MM-dd"),
                NameOfSupplier = pb.ClientSupplier,
                Description = pb.ItemDescription,
                PoNumber = pb.PONumber,
                Apv = pb.VoucherJournal,
                RrNumber = pb.RRNumber,
                ReceiptNumber = pb.ReferenceNo,
                Amount = pb.LineAmount,
                NameOfAccount = pb.AccountTitle,
                Debit = pb.LineAmount > 0 ? pb.LineAmount.Value : 0,
                Credit = pb.LineAmount < 0 ? pb.LineAmount.Value : 0,
                DRCR = pb.DRCP
            });

            return Result.Success(result);
        }
    }
}