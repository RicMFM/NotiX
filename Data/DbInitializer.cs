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



            // Se não houver Categorias, cria-as
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


            // Se não houver Utilizadores Identity, cria-os
            var users = Array.Empty<Utilizadores>();
            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            if (!dbContext.Users.Any())
            {
                users = [
                   new Utilizadores{Nome="admin", Contacto="931313131",UserName="admin@mail.pt", NormalizedUserName="ADMIN@MAIL.PT",
               Email="admin@mail.pt",NormalizedEmail="ADMIN@MAIL.PT", EmailConfirmed=true,
               SecurityStamp="5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="c86d8254-dd50-44be-8561-d2d44d4bbb2f",
               PasswordHash=hasher.HashPassword(null,"*aA1234567") },
            new Utilizadores{Nome="jose", Contacto="921212121",UserName="email.sete@mail.pt", NormalizedUserName="EMAIL.SETE@MAIL.PT",
               Email="email.sete@mail.pt",NormalizedEmail="EMAIL.SETE@MAIL.PT", EmailConfirmed=true,
               SecurityStamp="TW49PF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="d8254c86-dd50-44be-8561-d2d44d4bbb2f",
               PasswordHash=hasher.HashPassword(null,"Aa0_aa") }
                   ];
                await dbContext.Users.AddRangeAsync(users);
                haAdicao = true;
            }

			// Fotos pré-definidas
			var foto1 = new Fotos("foto1.jpg");
			var foto2 = new Fotos("foto2.jpg");
			var foto3 = new Fotos("foto3.jpg");

			var noticias = Array.Empty<Noticias>();
			// Se não houver Notícias, cria-as
			if (!dbContext.Noticias.Any()) {
				noticias = [
					new Noticias { Titulo = "Noticia 1", Texto = "Descrição da Notícia 1", DataEscrita = DateTime.Now, CategoriaFK = categorias[0].Id, ListaFotos = [foto1] },
					new Noticias { Titulo = "Noticia 2", Texto = "Descrição da Notícia 2", DataEscrita = DateTime.Now, CategoriaFK = categorias[1].Id, ListaFotos = [foto2]},
					new Noticias { Titulo = "Noticia 3", Texto = "Descrição da Notícia 3", DataEscrita = DateTime.Now, CategoriaFK = categorias[2].Id, ListaFotos = [foto3]}
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