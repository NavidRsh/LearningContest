using LearningContest.Application;
using LearningContest.Infrastructure;
using LearningContest.Persistence;
using LearningContest.Api.Middleware;
using LearningContest.Api.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using LearningContest.Application.Extension;
using LearningContest.Application.Validation;
using MediatR;
using LearningContest.Application.Transaction;
using Sieve.Services;
using LearningContest.Application.Contracts;
using LearningContest.Persistence.Repositories;
using AutoMapper;
using LearningContest.Application.PublishStrategy;
using FluentValidation.AspNetCore;
using LearningContest.Application.Profiles;
using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Infrastructure.SMS;
using LearningContest.Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LearningContest.Domain.Common.User;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using LearningContest.Infrastructure.HttpCall;
using LearningContest.Application.Contracts.HttpCall;
using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerUI;
using LearningContest.Domain.Common;
using LearningContest.Application.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using LearningContest.Application.Contracts.Persistence.Dapper;
using LearningContest.Persistence.Repositories.Dapper;
using Confluent.Kafka;
using Serilog;
using Elastic.Apm.AspNetCore;
using CommonInfrastructure.Access.Authorization;

namespace LearningContest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //از کتابخانه زیر استفاده کردیم
            //Scrutor
            services.AddValidators();
            //********************************
            services.AddScoped<SieveProcessor>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            ConfigureJwtOptions(services, Configuration);

            services.AddApplicationServices(Configuration).AddKafkaServices(Configuration["Kafka:Url"]);
            services.AddInfrastructureServices(Configuration);
            services.AddPersistenceServices(Configuration);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddScoped<Publisher>();
            services.AddScoped<LearningContestAuthorizeAttribute>(x => new LearningContestAuthorizeAttribute(CommonInfrastructure.Access.AccessItemType.Apps_Distributor));
            services.AddScoped<LearningContestAuthorizeFilter>(x => new LearningContestAuthorizeFilter(null));

            LearningContestAuthorize.Initialize(Configuration.GetConnectionString("LearningContestConnection"));

            services.AddScoped<ISendSMSService, SendSMSService>();
            services.AddScoped<IHttpCallService, HttpCallService>();
            services.AddScoped<IInstrumentService, InstrumentService>();
            //services.AddHostedService<TreasuryBillsMonitorHostedService>();
                        

            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddScoped<IMarketConversionService, MarketConversionService>();
            services.AddScoped<IFinancialFormulaService, FinancialFormulaService>();

            services.AddControllers()
                .AddFluentValidation(s =>
            {
                s.RegisterValidatorsFromAssemblyContaining<Startup>();
                s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearningContest " + DateTime.Now, Version = "v1", Description = @"
                Error Format : {Code:"""",Description:"""",Stack:""""}
                " });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                c.CustomSchemaIds(type => type.ToString());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearningContest v1");
                c.DocExpansion(DocExpansion.None);
            });
            //}

            app.UseElasticApm(Configuration);

            app.UseRouting();

            app.UseCustomExceptionHandler();

            app.UseCors("Open");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureJwtOptions(IServiceCollection services, IConfiguration configuration)
        {
            // Get options from app settings
            var authSettings = configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));

            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.TokenExpireMins = jwtAppSettingOptions[nameof(JwtIssuerOptions.TokenExpireMins)];
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents();
                configureOptions.Events.OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                };
                configureOptions.Events.OnChallenge = c =>
                {
                    //context.HandleResponse();
                    //context.Response.ContentType = "application/json";
                    //var payload = new { r = new { c = 201, m = context.ErrorDescription } };
                    //return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(payload));

                    c.HandleResponse();
                    c.Response.ContentType = "application/json";
                    StringValues token = "";
                    c.Request.Headers.TryGetValue("Authorization", out token);
                    if (token.Count == 0)
                    {
                        c.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    else
                    {
                        c.Response.StatusCode = 406;
                        LearningContest.Domain.Common.Error payload = new LearningContest.Domain.Common.Error(c.Error, c.ErrorDescription);
                        return c.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
                    }
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
                options.AddPolicy("IsActiveVisitor", policy => policy.RequireClaim("IsActive", "false"));
            });
        }

    }
}
