using System.Text;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Infrastructure.Mappers;
using NetFilm.Infrastructure.Services;
using NetFilm.Persistence.Data;

namespace NetFilm.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(ProfileMapper));
            });

            // DI for AWS Service
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IAWSService, AWSService>();

            // Injected User Service
			services.AddScoped<ICountryService, CountryService>();

            // DI for category
			services.AddScoped<ICategoryService, CategoryService>();

			// DI for comment
			services.AddScoped<ICommentService, CommentService>();

			// DI for Advertise
			services.AddScoped<IAdvertiseService, AdvertiseService>();

			// DI for movie
			services.AddScoped<IMovieService, MovieService>();

			// DI for subtitle
			services.AddScoped<ISubtitleService, SubtitleService>();

            // DI for service
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;

                //Email confirmation
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddTokenProvider<DataProtectorTokenProvider<User>>("NetFilm")
                .AddEntityFrameworkStores<NetFilmDbContext>()
                .AddDefaultTokenProviders();

            // Inject Authentication
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = configuration["Jwt:Issuer"],
            //            ValidAudience = configuration["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            //        });

            // DI for service
            services.AddScoped<ICountryService, CountryService>();

            // Injected User Service
            services.AddScoped<IUserService, UserService>();

            // Injected Role Service
            services.AddScoped<IRoleService, RoleService>();

            // Inject Auth Service
            services.AddScoped<IAuthService, AuthService>();

            // Inject Email Service
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
