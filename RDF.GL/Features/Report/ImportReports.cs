using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Report;

[Route("api/reports")]

public class ImportReports(IMediator _mediator) : ControllerBase
{

    [AllowAnonymous]
    [RequestSizeLimit(100_000_000)]
    [HttpPost("import")]
    public async Task<IActionResult> Import([FromBody] ImportReportsCommand command)
    {
        //if (User.Identity is ClaimsIdentity identity
        //       && int.TryParse(identity.FindFirst("id")?.Value, out var userId))
        //{
        //    command.AddedBy = userId;
        //}
        if (command == null)
        {
            return BadRequest("Request body is missing or invalid");
        }

        var result = await _mediator.Send(command);
        if (result.IsFailure || result.IsWarning && !result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    public class ImportReportsCommand : IRequest<Result>
    {
        public ICollection<ImportReport> Reports { get; set; }
        public int AddedBy { get; set; }
    }

    public class ImportReport
    {
        [Required] public string SyncId { get; set; }
        public string Mark1 { get; set; }
        public string Mark2 { get; set; }
        public string AssetCIP { get; set; }
        public string AccountingTag { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ClientSupplier { get; set; }
        public string AccountTitleCode { get; set; }
        public string AccountTitle { get; set; }
        public string CompanyCode { get; set; }
        public string Company { get; set; }
        public string DivisionCode { get; set; }
        public string Division { get; set; }
        public string DepartmentCode { get; set; }
        public string Department { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string SubUnitCode { get; set; }
        public string SubUnit { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public string PONumber { get; set; }
        public string RRNumber { get; set; }
        public string ReferenceNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineAmount { get; set; }
        public string VoucherJournal { get; set; }
        public string AccountType { get; set; }
        public string DRCR { get; set; }
        public string AssetCode { get; set; }
        public string Asset { get; set; }
        public string ServiceProviderCode { get; set; }
        public string ServiceProvider { get; set; }
        public string BOA { get; set; }
        public string Allocation { get; set; }
        public string AccountGroup { get; set; }
        public string AccountSubGroup { get; set; }
        public string FinancialStatement { get; set; }
        public string UnitResponsible { get; set; }
        public string Batch { get; set; }
        public string Remarks { get; set; }
        public string PayrollPeriod { get; set; }
        public string Position { get; set; }
        public string PayrollType { get; set; }
        public string PayrollType2 { get; set; }
        public string DepreciationDescription { get; set; }
        public string RemainingDepreciationValue { get; set; }
        public string UsefulLife { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Particulars { get; set; }
        public string Month2 { get; set; }
        public string FarmType { get; set; }
        public string JeanRemarks { get; set; }
        public string From { get; set; }
        public string ChangeTo { get; set; }
        public string Reason { get; set; }
        public string CheckingRemarks { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public string ChequeVoucherNumber { get; set; }
        public string BOA2 { get; set; }
        public string System { get; set; }
        public string Books { get; set; }

    }

    public class ImportReportResult
    {
        public List<ImportReport> DuplicateReports { get; set; }
        public List<ImportReport> ForSyncingReports { get; set; }

        public ImportReportResult(List<ImportReport> duplicateReports, List<ImportReport> forSyncingReports)
        {
            DuplicateReports = duplicateReports;
            ForSyncingReports = forSyncingReports;
        }
    }

    public class Handler(ProjectGLDbContext context) : IRequestHandler<ImportReportsCommand, Result>
    {
        public async Task<Result> Handle(ImportReportsCommand request, CancellationToken cancellationToken)
        {
            var duplicateReport = new List<ImportReport>();
            var forSyncingReport = new List<ImportReport>();

            var alreadySyncedSyncIds = await context.GeneralLedgers
                .Where(reports => request.Reports.Select(r => r.SyncId).Contains(reports.SyncId))
                .Select(reports => reports.SyncId)
                .ToListAsync(cancellationToken);

            foreach (var item in request.Reports)
            {
                if (alreadySyncedSyncIds.Contains(item.SyncId))
                {
                    duplicateReport.Add(item);
                }
                else
                {
                    forSyncingReport.Add(item);
                }
            }

            var newReport = request.Reports.Where(item => !duplicateReport.Contains(item))
                .Select(item => new GeneralLedger
                {
                    SyncId = item.SyncId,
                    Mark1 = item.Mark1,
                    Mark2 = item.Mark2,
                    AssetCIP = item.AssetCIP,
                    AccountingTag = item.AccountingTag,
                    TransactionDate = item.TransactionDate,
                    ClientSupplier = item.ClientSupplier,
                    AccountTitleCode = item.AccountTitleCode,
                    AccountTitle = item.AccountTitle,
                    CompanyCode = item.CompanyCode,
                    Company = item.Company,
                    DivisionCode = item.DivisionCode,
                    Division = item.Division,
                    DepartmentCode = item.DepartmentCode,
                    Department = item.Department,
                    UnitCode = item.UnitCode,
                    Unit = item.Unit,
                    SubUnitCode = item.SubUnitCode,
                    SubUnit = item.SubUnit,
                    LocationCode = item.LocationCode,
                    Location = item.Location,
                    PONumber = item.PONumber,
                    RRNumber = item.RRNumber,
                    ReferenceNo = item.ReferenceNo,
                    ItemCode = item.ItemCode,
                    ItemDescription = item.ItemDescription,
                    Quantity = item.Quantity,
                    UOM = item.UOM,
                    UnitPrice = item.UnitPrice,
                    LineAmount = item.LineAmount,
                    VoucherJournal = item.VoucherJournal,
                    AccountType = item.AccountType,
                    DRCP = item.DRCR,
                    AssetCode = item.AssetCode,
                    Asset = item.Asset,
                    ServiceProviderCode = item.ServiceProviderCode,
                    ServiceProvider = item.ServiceProvider,
                    BOA = item.BOA,
                    Allocation = item.Allocation,
                    AccountGroup = item.AccountGroup,
                    AccountSubGroup = item.AccountSubGroup,
                    FinancialStatement = item.FinancialStatement,
                    UnitResponsible = item.UnitResponsible,
                    Batch = item.Batch,
                    Remarks = item.Remarks,
                    PayrollPeriod = item.PayrollPeriod,
                    Position = item.Position,
                    PayrollType = item.PayrollType,
                    PayrollType2 = item.PayrollType2,
                    DepreciationDescription = item.DepreciationDescription,
                    RemainingDepreciationValue = item.RemainingDepreciationValue,
                    UsefulLife = item.UsefulLife,
                    Month = item.Month,
                    Year = item.Year,
                    Particulars = item.Particulars,
                    Month2 = item.Month2,
                    FarmType = item.FarmType,
                    JeanRemarks = item.JeanRemarks,
                    From = item.From,
                    ChangeTo = item.ChangeTo,
                    Reason = item.Reason,
                    CheckingRemarks = item.CheckingRemarks,
                    BankName = item.BankName,
                    ChequeNumber = item.ChequeNumber,
                    ChequeVoucherNumber = item.ChequeVoucherNumber,
                    BOA2 = item.BOA2,
                    System = item.System,
                    Books = item.Books,
                    AddedBy = 1
                });

            if (duplicateReport.Count > 0)
            {
                var combinedResult = new ImportReportResult(duplicateReport, forSyncingReport);
                return Result.Warning(combinedResult, "Validate data before import.");
            }

            await context.BulkInsertAsync(newReport, cancellationToken: cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}