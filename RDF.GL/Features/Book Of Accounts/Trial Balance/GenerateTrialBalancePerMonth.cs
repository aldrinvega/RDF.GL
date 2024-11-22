using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Trial_Balance;

public class GenerateTrialBalancePerMonth
{
    public class GenerateTrialBalancePerMonthCommand : IRequest<Result>
    {
        public string Search { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
    }
    
    public record GenerateTrialBalancePerMonthResponse
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? Amount { get; set; }
        public string ChartOfAccount { get; set; }
        public decimal? Decimal { get; set; }
        public decimal? Credit { get; set; }
        public string Drcr { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateTrialBalancePerMonthCommand, Result>
    {
        public async Task<Result> Handle(GenerateTrialBalancePerMonthCommand request,
            CancellationToken cancellationToken)
        {
            var trialBalance = await context.GeneralLedgers
                .Where(gl => gl.Month == request.Month)
                .Where(gl => gl.Year == request.Year)
                .Where(gl => gl.System == request.System)
                .Select(gl => new GenerateTrialBalancePerMonthResponse
                {
                    Date = gl.TransactionDate.ToString("MM/dd/yyyy"),
                    CustomerName = gl.ClientSupplier,
                    ReferenceNumber = gl.ReferenceNo,
                    Amount = gl.LineAmount > 0 ? gl.LineAmount : 0,
                    ChartOfAccount = gl.AccountTitle,
                    Decimal = gl.LineAmount > 0 ? gl.LineAmount : 0,
                    Credit = gl.LineAmount < 0 ? gl.LineAmount : 0,
                    Drcr = gl.DRCP
                })
                .ToListAsync(cancellationToken: cancellationToken);

            return Result.Success(trialBalance);
        }
    }
}