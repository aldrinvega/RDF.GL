using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;
using RDF.GL.Features.UserRole;

namespace RDF.GL.Features.Users;

[Route("api/user"), ApiController]

public class UpdateUser(IMediator _mediator) : ControllerBase
{

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Post([FromBody] UpdateUserCommand request, [FromRoute] int id)
    {
        try
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
    public class UpdateUserCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string IdPrefix { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserRoleId { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<UpdateUserCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return UserErrors.NotFound();
            }

            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == request.IdNumber && u.Id != request.Id, cancellationToken);

            if (userExist != null) 
            {
                return UserErrors.UserAlreadyExist(request.LastName + ", " + request.FirstName + " " + request.MiddleName);
            }

            var existingUserName = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Id != request.Id, cancellationToken);

            if (existingUserName != null)
            {
                return UserErrors.UsernameAlreadyExist(request.Username);
            }

            var validateUserRole = await _context.UserRoles.FirstOrDefaultAsync(u => u.Id == request.UserRoleId, cancellationToken);
            
            if(validateUserRole == null)
            {
                return UserRoleError.UserRoleNotFound();
            }
            
            user.IdPrefix = request.IdPrefix;
            user.IdNumber = request.IdNumber;
            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.Sex = request.Sex;
            user.Username = request.Username;
            user.UserRoleId = request.UserRoleId;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
           
        }
    }
}
