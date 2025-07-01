using NotiX.Models;
using Microsoft.AspNetCore.Identity;


namespace NotiX.Data
{

    internal class DbInitializer
    {

        internal static async void Initialize(ApplicationDbContext dbContext)
        {

            /*
             * https://stackoverflow.com/questions/70581816/how-to-seed-data-in-net-core-6-with-entity-framework
             * 
             * https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0#initialize-db-with-test-data
             * https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/data/ef-mvc/intro/samples/5cu/Program.cs
             * https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0300
             */


            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();

            // var auxiliar
            bool haAdicao = false;



            // Se não houver Cursos, cria-os
            var categorias = Array.Empty<Categorias>();
            if (!dbContext.Categorias.Any())
            {
                categorias = [
                   new Categorias{ Categoria="Musica"},
               new Categorias{ Categoria="Tecnologia"},
               new Categorias{ Categoria="Cultura"}
                //adicionar outros cursos
                ];
                await dbContext.Categorias.AddRangeAsync(categorias);
                haAdicao = true;
            }


            try
            {
                if (haAdicao)
                {
                    // tornar persistentes os dados
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
    }
}