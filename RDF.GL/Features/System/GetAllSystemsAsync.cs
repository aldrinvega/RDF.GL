using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.System;
[Route("api/system"), ApiController]

public class GetAllSystemsAsync(IMediator mediator) : ControllerBase
{
    [HttpGet("page")]
    public async Task<IActionResult> Get([FromQuery] GetAllSystemAsyncQuery request)
    {
        try
        {
            var result = await mediator.Send(request);
            
            Response.AddPaginationHeader(
                result.CurrentPage, 
                result.PageSize, 
                result.TotalCount, 
                result.TotalPages, 
                result.HasPreviousPage, 
                result.HasNextPage);

            var successResult = new
            {
                result,
                result.CurrentPage,
                result.PageSize,
                result.TotalCount,
                result.TotalPages,
                result.HasPreviousPage,
                result.HasNextPage
            };
            
            
            return Ok(successResult);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public class GetAllSystemAsyncQuery : UserParams, IRequest<PagedList<GetAllSystemsResponse>>
    {
        public string Search { get; set; }
        public bool? Status { get; set; }
    }

    public record GetAllSystemsResponse
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Endpoint { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
    }
    public class Handler(ProjectGLDbContext context) : IRequestHandler<GetAllSystemAsyncQuery, PagedList<GetAllSystemsResponse>>
    {
        public async Task<PagedList<GetAllSystemsResponse>> Handle(GetAllSystemAsyncQuery request, CancellationToken cancellationToken)
        {
            
            IQueryable<Domain.System> systems = context.Systems;

            if (!string.IsNullOrEmpty(request.Search))
            {
                systems = systems.Where(s => s.SystemName.Contains(request.Search));
            }

            if (request.Status != null)
            {
                systems = systems.Where(s => s.IsActive == request.Status);
            }

            var result = systems.Select(s => new GetAllSystemsResponse
            {
                Id = s.Id,
                SystemName = s.SystemName,
                Endpoint = s.Endpoint,
                Token = s.Token,
                IsActive = s.IsActive
            });
            
            return await PagedList<GetAllSystemsResponse>.CreateAsync(result, request.PageNumber, request.PageSize);
        }
    }
}