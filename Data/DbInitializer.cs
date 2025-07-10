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

            var categorias = dbContext.Categorias.ToList();

			// Fotos pré-definidas
			var foto1 = new Fotos("foto1.jpg");
			var foto2 = new Fotos("foto2.jpg");
			var foto3 = new Fotos("foto3.jpg");

			var noticias = Array.Empty<Noticias>();
			// Se não houver Notícias, cria-as
			if (!dbContext.Noticias.Any()) {
				noticias = [
					new Noticias { Titulo = "Noticia 1", Texto = "Descrição da Notícia 1", DataEscrita = DateTime.Now, Categoria = categorias[0], ListaFotos = [foto1] },
					new Noticias { Titulo = "Noticia 2", Texto = "Descrição da Notícia 2", DataEscrita = DateTime.Now, Categoria = categorias[1], ListaFotos = [foto2]},
					new Noticias { Titulo = "Noticia 3", Texto = "Descrição da Notícia 3", DataEscrita = DateTime.Now, Categoria = categorias[2], ListaFotos = [foto3]}
				];
				await dbContext.Noticias.AddRangeAsync(noticias);
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