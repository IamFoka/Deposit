using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Deposit.Data;
using Deposit.Data.Interfaces;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Deposit.WebApi
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
            services.AddDbContext<DepositDbContext>(
                options => options.UseMySql("server=localhost;port=3306;database=deposit;uid=ef;password=123")
            );
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IRepository<Product>, GenericRepository<Product>>();
            services.AddScoped<IRepository<Customer>, GenericRepository<Customer>>();
            services.AddScoped<IRepository<CustomerOrder>, GenericRepository<CustomerOrder>>();
            services.AddScoped<IRepository<CustomerOrderItem>, GenericRepository<CustomerOrderItem>>();
            services.AddScoped<IRepository<Provider>, GenericRepository<Provider>>();
            services.AddScoped<IRepository<ProviderOrder>, GenericRepository<ProviderOrder>>();
            services.AddScoped<IRepository<ProviderOrderItem>, GenericRepository<ProviderOrderItem>>();

            services.AddScoped<CustomerServices>();
            services.AddScoped<CustomerOrderServices>();
            services.AddScoped<CustomerOrderItemServices>();
            services.AddScoped<ProviderServices>();
            services.AddScoped<ProviderOrderServices>();
            services.AddScoped<ProviderOrderItemServices>();
            services.AddScoped<ProductServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
