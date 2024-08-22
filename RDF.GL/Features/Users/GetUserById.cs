using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;

[Route("api/user"), ApiController]

public class GetUserById(IMediator _mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var query = new GetUserByIdQuery
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
    public class GetUserByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetUserQueryResponse
    {
        public int Id { get; set; }
        public string IdPrefix { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Sex { get; set; }
        public string UserRole { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<GetUserByIdQuery, Result>
    {
        public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context
                .Users
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound();
            }

            var result = new GetUserQueryResponse
            {
                Id = user.Id,
                IdPrefix = user.IdPrefix,
                IdNumber = user.IdNumber,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Username = user.Username,
                Sex = user.Sex,
                UserRole = user.UserRole.UserRoleName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt.ToString("MM/dd/yyyy h:mm:ss tt"),
                UpdatedAt = user.UpdatedAt.HasValue ? user.UpdatedAt.Value.ToString("MM/dd/yyyy h:mm:ss tt") : null
            };

            return Result.Success(result);
        }
    }
}
