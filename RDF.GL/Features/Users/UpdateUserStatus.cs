using MediatR;
using Microsoft.AspNetCore.Mvc;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;

[Route("api/user"), ApiController]

public class UpdateUserStatus(IMediator _mediator) : ControllerBase
{
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch([FromBody] UpdateUserStatusCommand request, [FromRoute] int id)
    {
        try
        {
            request.Id = id;
            var result = await _mediator.Send(request);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class UpdateUserStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<UpdateUserStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == request.Id);

            if (user == null)
            {
                return UserErrors.NotFound();
            }

            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
