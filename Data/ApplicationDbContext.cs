using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotiX.Models;

namespace NotiX.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Criar os ROLES
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "a", Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "f", Name = "funcionario", NormalizedName = "FUNCIONARIO" }
            );

            // Criar utilizador admin
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "admin",
                    UserName = "admin@mail.pt",
                    NormalizedUserName = "ADMIN@MAIL.PT",
                    Email = "admin@mail.pt",
                    NormalizedEmail = "ADMIN@MAIL.PT",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hasher.HashPassword(null, "*aA1234567")
                }
            );

			// Criar utilizador funcionário
			builder.Entity<IdentityUser>().HasData(
				new IdentityUser {
					Id = "funcionario",
					UserName = "func@func.pt",
					NormalizedUserName = "FUNC@FUNC.PT",
					Email = "func@func.pt",
					NormalizedEmail = "FUNC@FUNC.PT",
					EmailConfirmed = true,
					SecurityStamp = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString(),
					PasswordHash = hasher.HashPassword(null, "*aA1234567")
				}
			);

			// Associar utilizador à role Admin
			builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "admin", RoleId = "a" });

			// Associar utilizador à role Funcionario
			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string> { UserId = "funcionario", RoleId = "f" });


			// Criar categorias
			builder.Entity<Categorias>().HasData(
				new Categorias { Id = 1, Categoria = "Música" },
				new Categorias { Id = 2, Categoria = "Cultura" },
				new Categorias { Id = 3, Categoria = "Tecnologia" }
			);
		}

		/// <summary>
		/// tabela Utilizadores
		/// </summary>
		public DbSet<Utilizadores> Utilizadores { get; set; }
		/// <summary>
		/// tabela Fotos
		/// </summary>
		public DbSet<Fotos> Fotos { get; set; }
        /// <summary>
        /// tabela Categorias
        /// </summary>
        public DbSet<Categorias> Categorias { get; set; }
        /// <summary>
        /// tabela Noticias
        /// </summary>
        public DbSet<Noticias> Noticias { get; set; }

    }
}
