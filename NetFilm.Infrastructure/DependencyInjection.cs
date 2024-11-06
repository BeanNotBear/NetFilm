﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
        .AddEntityFrameworkStores<NetFilmDbContext>()
        .AddDefaultTokenProviders();

            // DI for service
            services.AddScoped<ICountryService, CountryService>();

            // Injected User Service


            // Injected Role Service
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
