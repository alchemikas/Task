using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Contract.Request;
using Product.Api.Contract.Response;
using Product.Api.DomainCore.Repository;
using Product.Api.Handlers;
using Product.Api.Handlers.Command;
using Product.Api.Handlers.Query;
using Product.Api.Infrastructure;
using Product.Api.Infrastructure.Repository;

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


            services.AddScoped<IProductRepository, ProductRepository>();
            // query
            services.AddScoped<IQueryHandler<GetProductRequest, ProductResponse>, GetProductHandler>();
            services.AddScoped<IQueryHandler<GetProductsRequest, ProductsResponse>, GetProductsHandler>();

            services.AddScoped<IQueryDispatcher, QueryDispatcher>(x =>
            {
                var buildedServices = services.BuildServiceProvider();
                return new QueryDispatcher(buildedServices);
            });

            // command
//            services.AddScoped<IQueryHandler<GetProductRequest, ProductResponse>, GetProductHandler>();

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
