using RDF.GL.Common;

namespace RDF.GL.Features.Authenticate;

public class AuthenticateErrors
{
    public static Error UsernamePasswordIncorrect() =>
        new("Authenticate.UsernamePasswordIncorrect", "Username or password is incorrect!");
    public static Error UnauthorizedAccess() =>
        new("Authenticate.UnauthorizedAccess", "You are not authorized to log in.");
    public static Error NoRole() =>
        new("Authenticate.NoRole", "There is no role assigned to this user. Contact admin");
}
