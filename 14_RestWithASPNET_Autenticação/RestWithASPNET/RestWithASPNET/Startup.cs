using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Configurations;
using RestWithASPNET.Hypermedia.Enricher;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Generic;
using RestWithASPNET.Sevices;
using RestWithASPNET.Sevices.Implementations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace RestWithASPNET
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
        }    

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            #region CONFIGURACOES PARA AUTHENTICATION

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                    Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            #endregion

            #region CONFIG DE CORS

            services.AddCors(options => options.AddDefaultPolicy(builder => {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            #endregion

            services.AddControllers();

            //criação do service do contexto de banco de dados----------------------
            //["Busca-se a correspondencia no appsettings.json"]
            var connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(
                connection, new MySqlServerVersion(new Version(8, 0, 26)))
            );

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml").ToString());
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json").ToString());
            }).AddXmlSerializerFormatters();

            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            services.AddSingleton(filterOptions);

            //Versionamento API
            services.AddApiVersioning();

            //Swagger
            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Rest API",
                    Version = "v1",
                    Description = "Rest API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Emerson Sampaio",
                        Url = new Uri("http://google.com.br")
                    }
                }) ;
            });

            //Dependency injection
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();
            services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();

            services.AddTransient<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //HABILITANDO CORS PARA EVITAR RESTRIÇÃO DE ACESSO
            app.UseCors();


            #region Swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$","swagger");
            app.UseRewriter(option);

            #endregion 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");
            });
        }

        private void MigrateDatabase(string connection)
        {
            try
            {
                //usei de uma forma diferente // Aula 83
                var evolveConnection = new MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/datasets" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}
