﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Book_Of_Accounts.Sales_Journal;
[Route("api/bookofaccounts"), ApiController]

public class GenerateSalesJournalPerMonth(IMediator mediator) : ControllerBase
{
    [HttpGet("salesjournal")]
    public async Task<IActionResult> GetSalesJournalPerMonth([FromQuery] GenerateSalesJournalPerMonthCommand request)
    {
        try
        {
            var salesJournal = await mediator.Send(request);
            return salesJournal.IsFailure ? BadRequest(salesJournal) : Ok(salesJournal);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class GenerateSalesJournalPerMonthCommand : IRequest<Result>
    {
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string FromYear { get; set; }
    }

    public record GenerateSalesJournalPerMonthResponse
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public string ChartOfAccount { get; set; }
        public decimal? LineAmount { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GenerateSalesJournalPerMonthCommand, Result>
    {
        public async Task<Result> Handle(GenerateSalesJournalPerMonthCommand request, CancellationToken cancellationToken)
        {
            
            var salesJournal = (await context.GeneralLedgers
                .Where(gl => string.Compare(gl.Year + "-" + gl.Month, request.FromYear + "-" + request.FromMonth) >= 0
                             && string.Compare(gl.Year + "-" + gl.Month, request.ToYear + "-" + request.ToMonth) <= 0)
                .Where(gl => gl.BOA == "Sales Journal")
                .ToListAsync(cancellationToken: cancellationToken))
                .ToList();
            
            var result = salesJournal.Select(sj => new GenerateSalesJournalPerMonthResponse
            {
                Date = sj.TransactionDate.ToString("MM/dd/yyyy"),
                CustomerName = sj.ClientSupplier,
                ReferenceNumber = sj.ReferenceNo,
                ChartOfAccount = sj.AccountTitle,
                LineAmount = sj.LineAmount,
                Debit = sj.LineAmount > 0 ? sj.LineAmount : 0,
                Credit = sj.LineAmount < 0 ? sj.LineAmount : 0,
            }).ToList();


            return Result.Success(result);
        }
    }
}