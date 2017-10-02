using System.Linq;
using EfContextPooling.Data;
using GenFu;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EfContextPooling
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
            var connection =
                @"Server=(localdb)\mssqllocaldb;Database=EfLikeOperator.EmployeeContext;Trusted_Connection=True;";

            services.AddDbContextPool<EmployeeContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EmployeeContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //Initialize and seed the database if it is empty
            dbContext.Database.EnsureCreated();
            if (!dbContext.Employees.Any())
            {
                A.Configure<Employee>()
                    .Fill(e => e.Id, 0)
                    .Fill(e => e.IsDeleted)
                    .WithRandom(new[] { true, true, true, false }) //Gives us a distribution of approx 25% deleted employees
                    .Fill(e => e.CompanyId).WithRandom(new[] { 1, 2, 3 }) //3 different companies
                    .Fill(e => e.Title).AsPersonTitle()
                    .Fill(e => e.LastName).WithRandom(new[] {"Chambers", "Timms", "Tibbs", "Tibbsington", "Paquette", "McTibbs" });


            
                var randomEmployees = A.ListOf<Employee>(500);

                dbContext.Employees.AddRange(randomEmployees);
                dbContext.SaveChanges();
            }

        }
    }
}
