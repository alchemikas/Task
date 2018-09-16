using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Handlers;
using Product.Api.DomainCore.Handlers.BaseHandler;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;
using Product.Api.Infrastructure;
using Product.Api.Infrastructure.Repository;
using Product.Api.Infrastructure.Services;
using Product.Api.LocalInfrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Product.Api.LocalInfrastructure.Dispachers;
using Product.Api.ReadModel.Handlers;
using Product.Api.ReadModel.Queries;


namespace Product.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services
            services.AddScoped<IProductExportService, ProductExportService>();
            services.AddSingleton<IImageFileResizeService, ImageFileResizeService>();

            //repo
            services.AddScoped<IWriteOnlyProductRepository, WriteOnlyProductRepository>();
            services.AddScoped<IReadOnlyProductRepository, ReadOnlyProductRepository>();
            //commands
            services.AddScoped<ICommandHander<CreateProductCommand>, CreateProductHandler>();
            services.AddScoped<ICommandHander<DeleteProductCommand>, DeleteProductHandler>();
            services.AddScoped<ICommandHander<UpdateProductCommand>, UpdateProductHandler>();

            // query
            services.AddScoped<IQueryHandler<GetProductQuery, ProductDetailsResponse>, GetProductHandler>();
            services.AddScoped<IQueryHandler<GetProductsQuery, GetProductsResponse>, GetProductsHandler>();
            services.AddScoped<IQueryHandler<ExportProductQuery, ReadModel.Queries.Responses.File>, ExportProductsHandler>();
            services.AddScoped<IQueryHandler<GetProductQuery, bool>, DoesProductExistHandler>();

            services.AddScoped<IQueryDispatcher, QueryDispatcher>(x =>
            {
                var buildedServices = services.BuildServiceProvider();
                return new QueryDispatcher(buildedServices);
            });

            services.AddScoped<ICommandDispacher, CommandDispacher>(x =>
            {
                var buildedServices = services.BuildServiceProvider();
                return new CommandDispacher(buildedServices);
            });

            services.AddAutoMapper();
            services.AddMvc(setupAction => { setupAction.ReturnHttpNotAcceptable = true; }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Product API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseMvc();
        }
    }
}
