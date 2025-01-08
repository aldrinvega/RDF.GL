using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.Report;
[Route("api/report"), ApiController]

public class GetAllGLReportAsync(IMediator _mediator) : ControllerBase
{
    [HttpGet("general-ledger")]
    public async Task<IActionResult> Get([FromQuery] GetAllGLReportQuery query)
    {
        try
        {
            var reports = await _mediator.Send(query);

            Response.AddPaginationHeader(
                reports.CurrentPage,
                reports.PageSize,
                reports.TotalCount,
                reports.TotalPages,
                reports.HasPreviousPage,
                reports.HasNextPage
            );

            var result = new
            {
                reports,
                reports.CurrentPage,
                reports.PageSize,
                reports.TotalCount,
                reports.TotalPages,
                reports.HasPreviousPage,
                reports.HasNextPage
            };
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class GetAllGLReportQuery : UserParams, IRequest<PagedList<GetAllGlReportResponse>>
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
        public string Search { get; set; }
    }

    public class GetAllGlReportResponse
    {
        public string SyncId { get; set; }
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
        public string DRCP { get; set; }
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
        public string ChequeNumber  { get; set; }
        public string ChequeVoucherNumber { get; set; }
        public string BOA2 { get; set; }
        public string System { get; set; }
        public string Books { get; set; }
    }

    public class Handler(ProjectGLDbContext _context, IHttpClientFactory _factory) : IRequestHandler<GetAllGLReportQuery, PagedList<GetAllGlReportResponse>>
    {
        public async Task<PagedList<GetAllGlReportResponse>> Handle(GetAllGLReportQuery request, CancellationToken cancellationToken)
        {

            var report = _context.GeneralLedgers
                .Where(report => report.Month == request.Month && report.Year == request.Year);

            if (!string.IsNullOrEmpty(request.Search))
            {
                report = report.Where(generalLedger => generalLedger.Books.Contains(request.Search));
            }

            if (!string.IsNullOrEmpty(request.System))
            {
                report = report.Where(generalLedger => generalLedger.System == request.System);
            }

            var result = report.Select(report => new GetAllGlReportResponse
            {
                SyncId = report.SyncId,
                Mark1 = report.Mark1,
                Mark2 = report.Mark2,
                AssetCIP = report.AssetCIP,
                AccountingTag = report.AccountingTag,
                TransactionDate = report.TransactionDate,
                ClientSupplier = report.ClientSupplier,
                AccountTitleCode = report.AccountTitleCode,
                AccountTitle = report.AccountTitle,
                CompanyCode = report.CompanyCode,
                Company = report.Company,
                DivisionCode = report.DivisionCode,
                Division = report.Division,
                DepartmentCode = report.DepartmentCode,
                Department = report.Department,
                UnitCode = report.UnitCode,
                Unit = report.Unit,
                SubUnitCode = report.SubUnitCode,
                SubUnit = report.SubUnit,
                LocationCode = report.LocationCode,
                Location = report.Location,
                PONumber = report.PONumber,
                RRNumber = report.RRNumber,
                ReferenceNo = report.ReferenceNo,
                ItemCode = report.ItemCode,
                ItemDescription = report.ItemDescription,
                Quantity = report.Quantity,
                UOM = report.UOM,
                UnitPrice = report.UnitPrice,
                LineAmount = report.LineAmount,
                VoucherJournal = report.VoucherJournal,
                AccountType = report.AccountType,
                DRCP = report.DRCP,
                AssetCode = report.AssetCode,
                Asset = report.Asset,
                ServiceProviderCode = report.ServiceProviderCode,
                ServiceProvider = report.ServiceProvider,
                BOA = report.BOA,
                Allocation = report.Allocation,
                AccountGroup = report.AccountGroup,
                AccountSubGroup = report.AccountSubGroup,
                FinancialStatement = report.FinancialStatement,
                UnitResponsible = report.UnitResponsible,
                Batch = report.Batch,
                Remarks = report.Remarks,
                PayrollPeriod = report.PayrollPeriod,
                Position = report.Position,
                PayrollType = report.PayrollType,
                PayrollType2 = report.PayrollType2,
                DepreciationDescription = report.DepreciationDescription,
                RemainingDepreciationValue = report.RemainingDepreciationValue,
                UsefulLife = report.UsefulLife,
                Month = report.Month,
                Year = report.Year,
                Particulars = report.Particulars,
                Month2 = report.Month2,
                FarmType = report.FarmType,
                JeanRemarks = report.JeanRemarks,
                From = report.From, 
                ChangeTo = report.ChangeTo,
                Reason = report.Reason,
                CheckingRemarks = report.CheckingRemarks,
                BankName = report.BankName,
                ChequeNumber = report.ChequeNumber,
                ChequeVoucherNumber = report.ChequeVoucherNumber,
                BOA2 = report.BOA2,
                System = report.System,
                Books = report.Books
            });

            return await PagedList<GetAllGlReportResponse>.CreateAsync(result, request.PageNumber, request.PageSize);


        }
    }

}
