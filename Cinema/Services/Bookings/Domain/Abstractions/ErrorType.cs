namespace Domain.Abstractions;

public enum ErrorType
{
    Failure = 0,
    Problem = 1,
    NotFound = 2,
    Conflict = 3,
    Validation = 4,
}