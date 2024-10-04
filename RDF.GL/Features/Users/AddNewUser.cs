using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;
[Route("api/user")]
[ApiController]
public class AddNewUser : ControllerBase
{
    private readonly IMediator _mediator;

    public AddNewUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddNewUserCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    public class AddNewUserCommand : IRequest<Result>
    {
        public string IdPrefix { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Username { get; set; }
        public int? UserRoleId { get; set; }
    }

    public class Handler : IRequestHandler<AddNewUserCommand, Result>
    {
        private readonly ProjectGLDbContext _context;

        public Handler(ProjectGLDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            var existingFullname = await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == request.IdNumber, cancellationToken);

            if (existingFullname != null)
            {
                return UserErrors.UserAlreadyExist(request.LastName + ", " + request.FirstName + " " + request.MiddleName);
            }

            if (user != null)
            {
                return UserErrors.UsernameAlreadyExist(request.Username);
            }

            var newUser = new Domain.Users
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Username),
                Sex = request.Sex,
                Username = request.Username,
                UserRoleId = request.UserRoleId,
                IdNumber = request.IdNumber,
                IdPrefix = request.IdPrefix
            };

            await _context.Users.AddAsync(newUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
