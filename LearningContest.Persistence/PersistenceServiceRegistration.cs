using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Application.Contracts.Persistence.Dapper;
using LearningContest.Domain.Common;
using LearningContest.Persistence.Repositories;
using LearningContest.Persistence.Repositories.Dapper;
using LearningContest.Persistence.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using LinqToDB.EntityFrameworkCore;

namespace LearningContest.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LearningContestDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LearningContestConnection")).LogTo(Console.WriteLine,Microsoft.Extensions.Logging.LogLevel.Information);
                
            });

            //services.AddDbContext<AlborzDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("AlborzConnection")));

            services.AddScoped<IUnitOfWorkLearningContest, UnitOfWorkLearningContest>();
            services.AddScoped<IUnitOfWorkAlborz, UnitOfWorkAlborz>();

            services.AddSingleton(s => new AlborzDapperRepository(configuration.GetConnectionString("AlborzConnection")));
            services.AddSingleton(s => new LearningContestDapperRepository(configuration.GetConnectionString("LearningContestConnection")));

            //services.AddTransient<IDapperRepository>(s => new DapperRepository(configuration.GetConnectionString("LearningContestConnection")));

            services.AddTransient<Func<ConnectionType, IDapperRepository>>(serviceProvider =>
            key =>
            {
                switch (key)
                {
                    case ConnectionType.Alborz:
                        return serviceProvider.GetService<AlborzDapperRepository>();
                    case ConnectionType.LearningContest:
                        return serviceProvider.GetService<LearningContestDapperRepository>();
                    default:
                        return serviceProvider.GetService<LearningContestDapperRepository>();
                }
            });

            //Extension for using Bulk Operations 
            LinqToDBForEFTools.Initialize();

            //ساخت ویوها
            CreateOrAlterViewsFromDb.CreateOrAlter(configuration.GetConnectionString("AlborzConnection"));

            return services;
        }

      
    }
}
