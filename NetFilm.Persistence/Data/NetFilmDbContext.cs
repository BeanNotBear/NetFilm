using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Entities;

namespace NetFilm.Persistence.Data
{
	public class NetFilmDbContext : IdentityDbContext<

		User,
		IdentityRole<Guid>,
		Guid,
		IdentityUserClaim<Guid>,
		IdentityUserRole<Guid>,
		IdentityUserLogin<Guid>,
		IdentityRoleClaim<Guid>,
		IdentityUserToken<Guid>
	>
	{
		public DbSet<Advertise> Advertises { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<MovieCategory> MovieCategories { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<MovieParticipant> MovieParticipants { get; set; }
		public DbSet<Vote> Votes { get; set; }

		public NetFilmDbContext(DbContextOptions<NetFilmDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure composite keys first
			modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
			modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(r => new { r.UserId, r.RoleId });
			modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

			// Then rename the tables
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
			modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
			modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
			modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
			modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
			modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

			// Your other entity configurations
			modelBuilder.Entity<MovieCategory>(entity =>
			{
				entity.HasKey(x => new { x.MovieId, x.CategoryId });
			});

			modelBuilder.Entity<MovieParticipant>(entity =>
			{
				entity.HasKey(x => new { x.ParticipantId, x.MovieId });
			});
		}
	}
}