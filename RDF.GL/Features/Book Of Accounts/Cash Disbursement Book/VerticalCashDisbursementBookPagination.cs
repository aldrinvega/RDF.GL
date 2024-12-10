﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using RDF.GL.Domain;

namespace RDF.GL.Features.Book_Of_Accounts.Cash_Disbursement_Book;

[Route("api/book-of-accounts"), ApiController]

public class VerticalCashDisbursementBookPagination(IMediator mediator) : ControllerBase
{
    [HttpGet("cash-disbursement-book/vertical/page")]
    public async Task<IActionResult> Get([FromQuery] VerticalCashDisbursementBookPaginationCommand request)
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
    public class VerticalCashDisbursementBookPaginationCommand : UserParams, IRequest<PagedList<CashDisbursementBookPaginationResponse>>
    {
        public string Search { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string System { get; set; }
    }

    public class CashDisbursementBookPaginationResponse
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

    public class Handler(ProjectGLDbContext context)
        : IRequestHandler<VerticalCashDisbursementBookPaginationCommand, PagedList<CashDisbursementBookPaginationResponse>>
    {
        public async Task<PagedList<CashDisbursementBookPaginationResponse>> Handle(
            VerticalCashDisbursementBookPaginationCommand request, CancellationToken cancellationToken)
        {
            var cashDisbursementBook = context.GeneralLedgers
                .Where(cgb => cgb.BOA == "Cash Disbursement Book" && cgb.Month == request.Month && cgb.Year == request.Year);

            if (!string.IsNullOrEmpty(request.Search))
            {
                cashDisbursementBook = cashDisbursementBook.Where(cgb => cgb.ItemDescription.Contains(request.Search));
            }

            var result = cashDisbursementBook.Select(cdb => new CashDisbursementBookPaginationResponse
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

            return await PagedList<CashDisbursementBookPaginationResponse>.CreateAsync(result, request.PageNumber,
                request.PageSize);

        }
    }
}