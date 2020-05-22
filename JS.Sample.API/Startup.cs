using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using JS.Sample.Application.Interfaces;
using JS.Sample.Application.Interfaces.Header;
using JS.Sample.Application.Interfaces.Queries;
using JS.Sample.Common.Domain;
using JS.Sample.Infratructure.Filters;
using JS.Sample.Infratructure.Idempotency;
using JS.Sample.Infratructure.Mapper;
using JS.Sample.Infratructure.Services;
using JS.Sample.Percistance;
using JS.Sample.QueryStack;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
namespace JS.Sample.API
{
    public class Startup
    {


        public Startup(IHostingEnvironment env)
        {
            // In ASP.NET Core 3.1 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection. Don't build or return
            // any IServiceProvider or the ConfigureContainer method
            // won't get called.

            services.AddScoped<IHeaderService, HeaderService>();
            services.AddScoped<ISampleQueries,SampleQueries>();
            services.AddMediatR(new[]
         {
                Assembly.Load("JS.Sample.CommandStack"),
                Assembly.Load("JS.Sample.QueryStack")
            });

            services.AddAutoMapper(c => c.AddProfile<SampleMappingProfile>(), typeof(Startup));


            services.AddRouting(options => options.LowercaseUrls = true);


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", coreBuilder => { coreBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
                

            });


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Configuration);


            services.AddScoped<ApiExceptionFilter>();

            services.AddCustomDbContext(Configuration);

            #region Swagger

            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddApiVersioning(o => o.ReportApiVersions = true);

            services.AddSwaggerGen(
              options =>
              {


                  options.DescribeAllEnumsAsStrings();
                  // resolve the IApiVersionDescriptionProvider service
                  // note: that we have to build a temporary service provider here because one has not been created yet
                  var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                  // add a swagger document for each discovered API version
                  // note: you might choose to skip or document deprecated API versions differently
                  foreach (var description in provider.ApiVersionDescriptions)
                  {
                      options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                  }

                  // add a custom operation filter which sets default values
                  options.OperationFilter<SwaggerDefaultValues>();



                  options.CustomSchemaIds(x => x.FullName);

                  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                  options.IncludeXmlComments(xmlPath);
              });
            #endregion

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers();

            services.AddOptions();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:

            builder.RegisterModule(new ApplicationModule(Configuration));

            builder.RegisterModule(new MediatorModule());

        }

        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(
          IApplicationBuilder app,
          ILoggerFactory loggerFactory,
          IApiVersionDescriptionProvider provider)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            app.UseSwagger();

            app.UseSwaggerUI(
              options =>
              {
                  // build a swagger endpoint for each discovered API version
                  foreach (var description in provider.ApiVersionDescriptions)
                  {
#if DEBUG
                      // For Debug in Kestrel
                      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
#else
						   // To use with Gateway
						   options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
#endif
                  }
              });

            app.UseMvc();

            app.UseCors();

        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Version = description.ApiVersion.ToString(),
                Title = $"JS Sample Microservices Project {description.ApiVersion}",
                Description = "A Sample dot net core Project using Microservices Clean Architecture",
                Contact = new OpenApiContact
                {
                    Name = "developer",
                    Email = "jsameniego@outlook.com"
                },

            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

    }
    internal static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEntityFrameworkSqlServer()
                 .AddDbContext<SampleDbContext>(options =>
                 {
                     options.UseSqlServer(configuration["DBConnection"],
                         sqlServerOptionsAction: sqlOptions =>
                         {
                             sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                         });
                 },
                     ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                 );



            return services;
        }

    }

    internal class ApplicationModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Resolve the dependencies
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //repositories dependencies

            builder.RegisterType(typeof(SampleDbContext)).As(typeof(IEntitiesContext)).InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>().As<IRequestManager>().InstancePerLifetimeScope();



        }
    }

    internal class MediatorModule : Autofac.Module
    {
        /// <summary>
        /// Magic
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
    .AsImplementedInterfaces().InstancePerLifetimeScope();




            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));



            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });




        }
    }
}
