using Microsoft.EntityFrameworkCore;
using RDF.GL.Data;

namespace RDF.Arcana.API.Common;

public static class MigrationExtentions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ProjectGLDbContext dbContext = scope.ServiceProvider.GetRequiredService<ProjectGLDbContext>();

        dbContext.Database.Migrate();
    }
}