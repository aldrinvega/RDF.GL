using RDF.GL.Common;

namespace RDF.GL.Features.System;

public class SystemErrors
{
    public static Error SystemAlreadyExist(string system) => new("System.AlreadyExist", $"{system} already exist");
    public static Error SystemNotFound() => new("System.NotFound", "System not found");
}