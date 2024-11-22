using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.System;

[Route("api/system"), ApiController]

public class AddNewSystem(IMediator mediator) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> Ádd([FromBody] AddNewSystemCommand command)
    {
        if (command == null)
        {
            return BadRequest("Request body is missing or invalid");
        }

        var result = await mediator.Send(command);
        
        return result.IsFailure ? BadRequest(result) : Ok(result);
    }

    public class AddNewSystemCommand: IRequest<Result>
    {
       public string SystemName { get; set; }
       public string Endpoint { get; set; }
       public string Token { get; set; }
    };

    public class Handler(ProjectGLDbContext context) : IRequestHandler<AddNewSystemCommand, Result>
    {
        public async Task<Result> Handle(AddNewSystemCommand request, CancellationToken cancellationToken)
        {
            var system = await context.Systems.FirstOrDefaultAsync(s => s.SystemName == request.SystemName, cancellationToken: cancellationToken);

            if (system is not null)
            {
                return Result.Failure(SystemErrors.SystemAlreadyExist(request.SystemName));
            }
            
            var newSystem = new Domain.System
            {
                SystemName = request.SystemName,
                Endpoint = request.Endpoint,
                Token = request.Token
            };

            await context.AddAsync(newSystem, cancellationToken: cancellationToken);
            await context.SaveChangesAsync(cancellationToken: cancellationToken);
            
            return Result.Success();
        }
    }
}