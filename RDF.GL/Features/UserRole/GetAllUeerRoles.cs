using System.Reflection.Metadata.Ecma335;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace RDF.GL.Features.UserRole;


[Route("api/role")]
[ApiController]
public class GetAllUserRoles(IMediator _mediator) : ControllerBase
{
    [HttpGet("page")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUserRoleQuery request)
    {
        try
        {
            var userRoles = await _mediator.Send(request);

            Response.AddPaginationHeader(
                userRoles.CurrentPage,
                userRoles.PageSize,
                userRoles.TotalCount,
                userRoles.TotalPages,
                userRoles.HasPreviousPage,
                userRoles.HasNextPage
                );

            var result = new
            {
                userRoles,
                userRoles.CurrentPage,
                userRoles.PageSize,
                userRoles.TotalCount,
                userRoles.TotalPages,
                userRoles.HasPreviousPage,
                userRoles.HasNextPage
            };

            var successResult = Result.Success(result);

            return Ok(successResult);
        }catch(Exception error)
        {
            return BadRequest(error.Message);
        }
        

    }
    public class GetAllUserRoleQuery : UserParams, IRequest<PagedList<GetAllUserRoleQueryResponse>>
    {
        public string Search { get; set; }
        public bool? Status { get; set; }
    }

    public class GetAllUserRoleQueryResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }
        public string CraetedAt { get; set; }
        public string UpdateAt { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<GetAllUserRoleQuery, PagedList<GetAllUserRoleQueryResponse>>
    {
        public async Task<PagedList<GetAllUserRoleQueryResponse>> Handle(GetAllUserRoleQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.UserRole> userRole = _context.UserRoles
                .Include(u => u.Users)
                .Include(au => au.AddedByUser)
                .Include(mu => mu.ModifiedByUser);

            if (!string.IsNullOrEmpty(request.Search))
            {
                userRole = userRole.Where(u => u.UserRoleName.Contains(request.Search));
            }

            if(request.Status != null)
            {
                userRole = userRole.Where(ur => ur.IsActive == request.Status);
            }

            var result = userRole.Select(ur => new GetAllUserRoleQueryResponse
            {
                Id = ur.Id,
                RoleName = ur.UserRoleName,
                Permissions = ur.Permissions,
                CraetedAt = ur.CreatedAt.Value.ToString("MM-dd-yyyy hh:mm tt"),
                UpdateAt = ur.UpdatedAt.Value.ToString("MM-dd-yyyy hh:mm tt"),
                AddedBy = ur.AddedByUser.LastName + ", " + ur.AddedByUser.FirstName + " " + ur.AddedByUser.MiddleName,
                ModifiedBy = ur.ModifiedByUser.LastName + ", " + ur.ModifiedByUser.FirstName + " " + ur.ModifiedByUser.MiddleName,
                IsActive = ur.IsActive
            });

            return await PagedList<GetAllUserRoleQueryResponse>.CreateAsync(result, request.PageNumber, request.PageSize);
                
        }
    }
}
