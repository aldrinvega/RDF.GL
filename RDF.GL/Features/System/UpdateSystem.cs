using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.System;

[Route("api/system"), ApiController]

public class UpdateSystem (IMediator mediator) : ControllerBase
{
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSystemCommand command)
    {
        try
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return result.IsFailure ? BadRequest(result) : Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    public class UpdateSystemCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Endpoint { get; set; }
        public string Token { get; set; }
    };
    
    public class Handler(ProjectGLDbContext context) : IRequestHandler<UpdateSystemCommand, Result>
    {
        public async Task<Result> Handle(UpdateSystemCommand request, CancellationToken cancellationToken)
        {
            var system = await context.Systems.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);
            var systemName = await context.Systems.FirstOrDefaultAsync(s => s.SystemName == request.SystemName,
                cancellationToken: cancellationToken); 

            if (system is null)
            {
                return Result.Failure(SystemErrors.SystemNotFound());
            }

            if (systemName is not null && systemName.Id != request.Id)
            {
                return Result.Failure(SystemErrors.SystemAlreadyExist(request.SystemName));
            }
            
            system.SystemName = request.SystemName;
            system.Endpoint = request.Endpoint;
            system.Token = request.Token;

            await context.SaveChangesAsync(cancellationToken: cancellationToken);

            return Result.Success();
        }
    }
}