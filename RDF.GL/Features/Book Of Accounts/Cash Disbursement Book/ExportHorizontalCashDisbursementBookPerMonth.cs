/*using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Cash_Disbursement_Book;

public class ExportHorizontalCashDisbursementBookPerMonth
{
    public class ExportHorizontalCashDisbursementBookPerMonthCommand : IRequest<Unit>
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
    }
    
    public record ExportHorizontalCashDisbursementBookPerMonthResponse
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
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<ExportHorizontalCashDisbursementBookPerMonthCommand, Unit>
    {
        public async Task<Unit> Handle(ExportHorizontalCashDisbursementBookPerMonthCommand request, CancellationToken cancellationToken)
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

            var cashDisbursementResult = await cashDisbursementBook
                .Select(sj => new ExportHorizontalCashDisbursementBookPerMonthResponse
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
                            new ExportHorizontalCashDisbursementBookPerMonthResponse.ChartOfAccount
                            {
                                NameOfAccount = accountGroup.Key,
                                Credit = accountGroup.Sum(g => g.LineAmount < 0 ? g.LineAmount : 0),
                                Debit = accountGroup.Sum(g => g.LineAmount > 0 ? g.LineAmount : 0),
                                DrCr = accountGroup.First().DRCP
                            })
                        .GroupBy(an => an.NameOfAccount)
                        .Select(accountGroup => new ExportHorizontalCashDisbursementBookPerMonthResponse.ChartOfAccount
                        {
                            NameOfAccount = accountGroup.Key,
                            Credit = accountGroup.Sum(g => g.Credit),
                            Debit = accountGroup.Sum(g => g.Debit),
                            DrCr = accountGroup.First().DrCr
                        }).ToList()
                }).ToListAsync(cancellationToken);

            var cashDisbursementResultList = cashDisbursementResult.ToList();   

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add($"Approved Move Order Report");

       

                var headers = new List<string>
                {
                    "Cheque Date",
                    "Bank",
                    "Cv Number",
                    "Cheque Number",
                    "Payee",
                    "Description",
                    "Tag Number",
                    "Apv Number",
                    "Account Name",
                    "Debit",
                    "Credit",
                    "DrCr"
                };


                var range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, headers.Count));

                range.Style.Fill.BackgroundColor = XLColor.Azure;
                range.Style.Font.Bold = true;
                range.Style.Font.FontColor = XLColor.Black;
                range.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                range.SetAutoFilter(true);
                range.Range("I1:J1").Row(1).Merge();

                for (var index = 1; index <= headers.Count; index++)
                {
                    worksheet.Cell(1, index).Value = headers[index - 1];
                }

                for (var index = 1; index <= cashDisbursementResult.Count; index++)
                {
                    var row = worksheet.Row(index + 1);

                    row.Cell(1).Value = cashDisbursementResult[index - 1].ChequeDate;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].Bank;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].CvNumber;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].ChequeNumber;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].Payee;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].Description;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].TagNumber;
                    row.Cell(1).Value = cashDisbursementResult[index - 1].ApvNumber;
                    foreach (var accounts in cashDisbursementResult[index - 1].AccountName.Distinct())
                    {
                        row.Cell(1).Value = accounts.NameOfAccount;
                        
                    }
                    

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs($"Approved Move Orders {request.DateFrom}-{request.DateTo}.xlsx");
                }


            }
        }
    }
}
*/