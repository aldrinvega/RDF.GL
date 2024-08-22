using RDF.GL.Common;

namespace RDF.Arcana.API.Features.Users;

public class UserErrors
{
    public static Error UsernameAlreadyExist(string username) =>
        new Error("User.UsernameAlreadyExist", $"{username} is already exist");

    public static Error UserAlreadyExist(string fullName) => new Error("User.AlreadyExist", $"{fullName} is already exist");
    public static Error NotFound() => new Error("User.NotFound", "User not found");
    public static Error OldPasswordIncorrect() => new Error("User.OldPassword", "Old password is incorrect");
}