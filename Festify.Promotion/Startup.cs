using Festify.Promotion.Acts;
using Festify.Promotion.Contents;
using Festify.Promotion.Data;
using Festify.Promotion.Shows;
using Festify.Promotion.Venues;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Festify.Promotion;

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
        services.AddControllersWithViews();

        services.AddDbContext<PromotionContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("PromotionContext"))
                .LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuting }));

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq();
        });

        //services.AddMassTransitHostedService();

        services.AddScoped<Dispatcher>();

        services.AddScoped<VenueQueries>();
        services.AddScoped<VenueCommands>();
        services.AddScoped<ActQueries>();
        services.AddScoped<ActCommands>();
        services.AddScoped<ShowQueries>();
        services.AddScoped<ShowCommands>();
        services.AddScoped<ContentQueries>();
        services.AddScoped<ContentCommands>();

        services.AddScoped<INotifier<Show>, ShowNotifier>();
        services.AddScoped<INotifier<ActDescription>, ActDescriptionNotifier>();
        services.AddScoped<INotifier<VenueDescription>, VenueDescriptionNotifier>();
        services.AddScoped<INotifier<VenueLocation>, VenueLocationNotifier>();
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
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}