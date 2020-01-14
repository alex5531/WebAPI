using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAPI.BLL;
using WebAPI.BLL.Contracts;
using WebAPI.BLL.Models;
using WebAPI.DAL.MSSQL;
using WebAPI.DAL.MSSQL.Contracts;
using System;
using WebAPI.DAL.MSSQL.Models;
using WebAPI.BLL.PasswordSecurity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebServiceExtensions
    {
        public static IServiceCollection AddWebServices(
            this IServiceCollection services,
            IConfigurationSection BLLOptionsSection,
            IConfigurationSection DALOptionSection)
        {
            if (BLLOptionsSection == null)
            {
                throw new ArgumentNullException(nameof(BLLOptionsSection));
            }

            if (DALOptionSection == null)
            {
                throw new ArgumentNullException(nameof(DALOptionSection));
            }

            var bllSettings = BLLOptionsSection.Get<UsersBLLOptions>();

            services.Configure<UsersBLLOptions>(opt =>
            {
                opt.JwtSecretKey = BLLOptionsSection.GetValue<string>("JwtSecretKey");
            });
            services.Configure<UsersMSSQLRepositoryOption>(opt =>
            {
                opt.UsersDbConnectionString = DALOptionSection.GetValue<string>("UsersDbConnectionString");
            });

            services.TryAddSingleton<IUsersRepository, UsersRepository>();
            services.TryAddSingleton<IPostsRepository, PostsRepository>();
            services.TryAddSingleton<IPasswordStorage, PasswordStorage>();

            services.TryAddScoped<IUsersService, UsersService>();
            services.TryAddScoped<IPostsService, PostsService>();
            services.TryAddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
