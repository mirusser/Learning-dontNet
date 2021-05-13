using DeskBooker.Core.DataInterfaces;
using DeskBooker.DataAccess;
using DeskBooker.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskBooker.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //TODO: move this method (EnsureDatabaseExists) outside startup
        private static void EnsureDatabaseExists(SqliteConnection connection)
        {
            var builder = new DbContextOptionsBuilder<DeskBookerContext>();
            builder.UseSqlite(connection);

            using var context = new DeskBookerContext(builder.Options);
            context.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            var connectionString = "DataSource=:memory:";
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            services.AddDbContext<DeskBookerContext>(options => options.UseSqlite(connection));

            EnsureDatabaseExists(connection);

            //repos
            services.AddTransient<IDeskRepository, DeskRepository>();
            services.AddTransient<IDeskBookingRepository, DeskBookingRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
