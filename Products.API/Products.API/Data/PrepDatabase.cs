using Microsoft.EntityFrameworkCore;

namespace Products.API.Data
{
    public static class PrepDatabase
    {
        public static void PrePopulation(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
            }

        }

        public static void SeedData(AppDbContext context)
        {
            if (!context.Database.CanConnect())
            {
                Console.WriteLine("Database does not exist. Running migrations...");
                context.Database.Migrate();
            }
            else
            {
                Console.WriteLine("Database already exists. Skipping migrations.");
            }
        }
    }
}
