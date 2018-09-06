using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Handlers.Command;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Handlers.Query;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;
using Product.Api.Infrastructure;
using Product.Api.Infrastructure.Dispachers;
using Product.Api.Infrastructure.Repository;
using Product.Api.Infrastructure.Services;
using Swashbuckle.AspNetCore.Swagger;
using File = Product.Api.DomainCore.Querys.Responses.File;

namespace Product.Api
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
            var connection = @"Server=.;Database=SchoolDB;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ProductContext>(options => options.UseSqlServer(connection));

            //services
            services.AddScoped<IProductExportService, ProductExportService>();
            services.AddScoped<IProductValidationService, ProductValidationService> ();
            services.AddScoped<IFileValidationService, FileValidationService> ();

            //repo
            services.AddScoped<IProductRepository, ProductRepository>();
            //commands
            services.AddScoped<ICommandHander<CreateProductCommand>, CreateProductHandler>();
            services.AddScoped<ICommandHander<DeleteProductCommand>, DeleteProductHandler>();
            services.AddScoped<ICommandHander<UpdateProductCommand>, UpdateProductHandler>();

            // query
            services.AddScoped<IQueryHandler<GetProductQuery, GetProductDetailsResponse>, GetProductHandler>();
            services.AddScoped<IQueryHandler<GetProductsQuery, GetProductsResponse>, GetProductsHandler>();
            services.AddScoped<IQueryHandler<ExportProductQuery, File>, ExportProductsHandler>();
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Product API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
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
