using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RDF.GL.Common;
using RDF.GL.Common.Extension;
using RDF.GL.Common.Pagination;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;

[Route("api/user")]
[ApiController]

public class GetAllUsers(IMediator _mediator) : ControllerBase
{
    [HttpGet("page")]
    public async Task<IActionResult> Get([FromQuery] GetAllUsersQuery request)
    {
        try
        {
            var users = await _mediator.Send(request);

            Response.AddPaginationHeader(
                users.CurrentPage,
                users.PageSize,
                users.TotalCount,
                users.TotalPages,
                users.HasPreviousPage,
                users.HasNextPage
            );

            var result = new
            {
                users,
                users.CurrentPage,
                users.PageSize,
                users.TotalCount,
                users.TotalPages,
                users.HasPreviousPage,
                users.HasNextPage
            };

            var successResult = Result.Success(result);

            return Ok(successResult);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class GetAllUsersQuery : UserParams, IRequest<PagedList<GetAllUsersResponse>>
    {
        public string Search { get; set; }
        public bool? Status { get; set; }
    }

    public class GetAllUsersResponse
    {
        public int Id { get; set; }
        public string IdPrefix { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Username { get; set; }
        public string UserRole { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class Handler(ProjectGLDbContext _context) : IRequestHandler<GetAllUsersQuery, PagedList<GetAllUsersResponse>>
    {
        public async Task<PagedList<GetAllUsersResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Users> users = _context.Users
                .Include(role => role.UserRole);

            if (!string.IsNullOrEmpty(request.Search))
            {
                users = users
                    .Where(u => u.FirstName.Contains(request.Search) ||
                                u.MiddleName.Contains(request.Search) ||
                                u.LastName.Contains(request.Search) ||
                                u.IdNumber.Contains(request.Search) ||
                                u.Username.Contains(request.Search) ||
                                u.UserRole.UserRoleName.Contains(request.Search));
            }

            if(request.Status != null)
            {
                users = users
                    .Where(u => u.IsActive == request.Status);
            }

            var result = users
                .Select(u => new GetAllUsersResponse
                {
                    Id = u.Id,
                    IdPrefix = u.IdPrefix,
                    IdNumber = u.IdNumber,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Sex = u.Sex,
                    Username = u.Username,
                    UserRole = u.UserRole.UserRoleName,
                    CreatedAt = u.CreatedAt.ToString("MM-dd-yyyy HH:mm:ss tt"),
                    UpdatedAt = u.UpdatedAt.Value.ToString("MM-dd-yyyy HH:mm:ss tt"),
                    IsActive = u.IsActive
                });

            return await PagedList<GetAllUsersResponse>.CreateAsync(result, request.PageNumber, request.PageSize);
        }
    }
}
