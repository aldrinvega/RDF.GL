using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RDF.GL.Common;
using RDF.GL.Data;
using RDF.GL.Domain;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RDF.GL.Features.Authenticate;

[Route("api/auth"), ApiController]
public class Authenticate : ControllerBase
{
    private readonly IMediator _mediator;

    public Authenticate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> AuthenticateUser(
       [FromBody] AuthenticateUserQuery request)
    {
        try
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
public sealed record AuthenticateUserQuery : IRequest<Result>
    {
        public AuthenticateUserQuery(string username)
        {
            Username = username;
        }

        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }

    public class AuthenticateUserResult
    {
        public AuthenticateUserResult(Domain.Users user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            Username = user.Username;
            RoleName = user.UserRole?.UserRoleName;
            Permission = user.UserRole?.Permissions;
            Token = token;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public ICollection<string> Permission { get; set; }
        public string Token { get; set; }
        public bool IsPasswordChanged { get; set; }
        public string ProfilePicture { get; set; }
    }

public class Handler : IRequestHandler<AuthenticateUserQuery, Result>
{

    private readonly IConfiguration _configuration;
    private readonly ProjectGLDbContext _context;

    public Handler(ProjectGLDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<Result> Handle(AuthenticateUserQuery command,
        CancellationToken cancellationToken)
    {

        var user = await _context.Users
            .Include(x => x.UserRole)
            .SingleOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

        //Verify if the credentials is correct
        if (user == null || !BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
        {
            return AuthenticateErrors.UsernamePasswordIncorrect();
        }

        if (user.IsActive == false)
        {
            return AuthenticateErrors.UnauthorizedAccess();
        }

        if (user.UserRoleId is null)
        {
            return AuthenticateErrors.NoRole();
        }

        await _context.SaveChangesAsync(cancellationToken);

        var token = GenerateJwtToken(user);

        var result = new AuthenticateUserResult(user, token);

        return Result.Success(result);
    }

    private string GenerateJwtToken(Domain.Users user)
    {
        var key = _configuration.GetValue<string>("JwtConfig:Key");
        var audience = _configuration.GetValue<string>("JwtConfig:Audience");
        var issuer = _configuration.GetValue<string>("JwtConfig:Issuer");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName)
                    }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
