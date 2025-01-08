using Microsoft.EntityFrameworkCore;
using RDF.GL.Data;

namespace RDF.Arcana.API.Common;

//Auto update dabtabase on startup

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ProjectGLDbContext dbContext = scope.ServiceProvider.GetRequiredService<ProjectGLDbContext>();

        dbContext.Database.Migrate();
    }
}   