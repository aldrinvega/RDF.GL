using RDF.GL.Common;

namespace RDF.GL.Features.UserRole;

public class UserRoleError
{
    public static Error UserRoleAlreadyExist(string userRole) =>
        new("UserRole.UserRoleAlreadyExist", $"{userRole} is already exist");

    public static Error UserRoleNotFound() =>
       new("UserRole.UserRoleAlreadyExist", "User Role not found");

    public static Error UserRoleIsInUse() =>
      new("UserRole.UserRoleIsInUse", "User Role is in use");
}
