using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.System;

[Route("api/system"), ApiController]

public class UpdateSystemStatus(IMediator mediator) : ControllerBase
{
    [HttpPatch("id")]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            var command = new UpdateSystemStatusCommand
            {
                Id = id
            };

            var result = await mediator.Send(command);

            return result.IsFailure ? BadRequest(result) : Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class UpdateSystemStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<UpdateSystemStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateSystemStatusCommand request, CancellationToken cancellationToken)
        {
            var system = await context.Systems.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

            if (system is null)
            {
                return Result.Failure(SystemErrors.SystemNotFound());
            }

            system.IsActive = !system.IsActive;

            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}