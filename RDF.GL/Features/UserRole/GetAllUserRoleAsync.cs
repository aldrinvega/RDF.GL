using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.UserRole;

[Route("api/role")]
[ApiController]

public class GetAllUserRoleAsync(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var query = new GetAlluserRoleAsyncQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class GetAlluserRoleAsyncQuery : IRequest<Result> { }

    public class GetAlluserRoelResponse
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

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<GetAlluserRoleAsyncQuery, Result>
    {
        public async Task<Result> Handle(GetAlluserRoleAsyncQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await _context.UserRoles
                .Include(user => user.AddedByUser)
                .Include(user => user.ModifiedByUser)
                .Where(ur => ur.IsActive == true).ToListAsync();

            var result = userRoles.Select(ur => new GetAlluserRoelResponse
            {
                Id = ur.Id,
                RoleName = ur.UserRoleName,
                Permissions = ur.Permissions,
                CraetedAt = ur.CreatedAt.Value.ToString("MM-dd-yyyy hh:mm tt"),
                UpdateAt = ur.UpdatedAt.HasValue ? ur.UpdatedAt.Value.ToString("MM-dd-yyyy hh:mm tt") : null,
                AddedBy = ur.AddedByUser != null
              ? ur.AddedByUser.LastName + ", " + ur.AddedByUser.FirstName + " " + (ur.AddedByUser.MiddleName ?? string.Empty)
              : null,
                ModifiedBy = ur.ModifiedByUser != null
                 ? ur.ModifiedByUser.LastName + ", " + ur.ModifiedByUser.FirstName + " " + (ur.ModifiedByUser.MiddleName ?? string.Empty)
                 : null,
                IsActive = ur.IsActive
            });

            return Result.Success(result);
        }
    }
}
