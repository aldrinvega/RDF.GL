using FluentValidation;
using MediatR;
using RDF.GL.Common.Exceptions;
using RDF.GL.Common.Messaging;

namespace RDF.Arcana.API.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures.Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage
            )).ToList();

        if (errors.Any())
        {
            throw new GL.Common.Exceptions.ValidationException(errors);
        }

        var response = await next();

        return response;
    }
}