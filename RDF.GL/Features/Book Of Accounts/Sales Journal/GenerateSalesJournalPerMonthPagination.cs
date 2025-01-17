﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Book_Of_Accounts.Sales_Journal;
[Route("api/bookofaccounts"), ApiController]

public class GenerateSalesJournalPerMonthPagination(IMediator mediator) : ControllerBase
{
    [HttpGet("salesjournal/page")]
    public async Task<IActionResult> Get([FromQuery] GenerateSalesJournalPerMonthPaginationCommand request)
    {
        try
        {
            var salesJournal = await mediator.Send(request);
            Response.AddPaginationHeader(
                salesJournal.CurrentPage, 
                salesJournal.PageSize, 
                salesJournal.TotalCount, 
                salesJournal.TotalPages,
                salesJournal.HasPreviousPage,
                salesJournal.HasNextPage
                );
            var result = new
            {
                salesJournal,
                salesJournal.CurrentPage, 
                salesJournal.PageSize, 
                salesJournal.TotalCount, 
                salesJournal.TotalPages,
                salesJournal.HasPreviousPage,
                salesJournal.HasNextPage
            };
            
            var successResult = Result.Success(result);

            return Ok(successResult);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class GenerateSalesJournalPerMonthPaginationCommand : UserParams,
        IRequest<PagedList<GenerateSalesJournalPerMonthPaginationResponse>>
    {
        public string Search { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string FromYear { get; set; }
    }

    public class GenerateSalesJournalPerMonthPaginationResponse
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public string ChartOfAccount { get; set; }
        public decimal? LineAmount { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string DrCr { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateSalesJournalPerMonthPaginationCommand, PagedList<GenerateSalesJournalPerMonthPaginationResponse>>
    {
        public async Task<PagedList<GenerateSalesJournalPerMonthPaginationResponse>> Handle(
            GenerateSalesJournalPerMonthPaginationCommand request, CancellationToken cancellationToken)
        {
            IQueryable<GeneralLedger> salesJournal = context.GeneralLedgers
                .Where(gl => string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                             && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0)
                .Where(gl => gl.BOA == "Sales Journal");
            
            var result = salesJournal
                .Select(sj => new GenerateSalesJournalPerMonthPaginationResponse
                {
                    Date = sj.TransactionDate.ToString("MM/dd/yyyy"),
                    CustomerName = sj.ClientSupplier,
                    ReferenceNumber = sj.ReferenceNo,
                    ChartOfAccount = sj.AccountTitle,
                    LineAmount = sj.LineAmount ?? 0,
                    Debit = sj.LineAmount > 0 ? sj.LineAmount.Value : 0,
                    Credit = sj.LineAmount < 0 ? sj.LineAmount.Value : 0,
                    DrCr = sj.DRCP
                });
            
            return await PagedList<GenerateSalesJournalPerMonthPaginationResponse>.CreateAsync(result, request.PageNumber, request.PageSize);
        }
    }
}