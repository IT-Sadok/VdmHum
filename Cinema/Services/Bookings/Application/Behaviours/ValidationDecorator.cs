namespace Application.Behaviours;

using Abstractions;
using Domain.Errors;
using Domain.Abstractions;
using FluentValidation;
using FluentValidation.Results;

internal static class ValidationDecorator
{
    private static async Task<ICollection<ValidationFailure>> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        var context = new ValidationContext<TCommand>(command);

        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    private static ValidationError CreateValidationError(ICollection<ValidationFailure> validationFailures) =>
        new(validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());

    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Count == 0)
            {
                return await innerHandler.HandleAsync(command, ct);
            }

            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken ct)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Count == 0)
            {
                return await innerHandler.HandleAsync(command, ct);
            }

            return Result.Failure(CreateValidationError(validationFailures));
        }
    }
}