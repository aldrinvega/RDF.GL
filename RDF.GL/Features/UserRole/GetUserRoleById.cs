using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.UserRole;
[Route("api/role"), ApiController]

public class GetUserRoleById(IMediator _mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var query = new GetUserRoleByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class GetUserRoleByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetUserRoleReponse
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; }
        public List<string> Permissions { get; set; }
        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public int? AddedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<GetUserRoleByIdQuery, Result>
    {
        public async Task<Result> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var userRole = await _context
                .UserRoles
                .Include(u => u.AddedByUser)
                .Include(u => u.ModifiedByUser)
                .FirstOrDefaultAsync(ur => ur.Id == request.Id, cancellationToken);

            if(userRole is null)
            {
                return UserRoleError.UserRoleNotFound();
            }

            var result = new GetUserRoleReponse
            {
                Id = userRole.Id,
                UserRoleName = userRole.UserRoleName,
                Permissions = userRole.Permissions,
                CreatedAt = userRole.CreatedAt.Value.ToString("MM-dd-yyyy hh:mm tt"),
                UpdatedAt = userRole.UpdatedAt.HasValue ? userRole?.UpdatedAt.Value.ToString("MM-dd-yyyy hh:mm tt") : null,
                AddedBy = userRole.AddedBy,
                ModifiedBy = userRole.ModifiedBy,
                IsActive = userRole.IsActive
            };

            return Result.Success(result);
        }
    }
}
