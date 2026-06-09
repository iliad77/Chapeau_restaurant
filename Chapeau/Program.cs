using Chapeau.Repositories;
using Chapeau.Repositories.Interface;
using Chapeau.Repositories.Interfaces;
using Chapeau.Service.Interface;
using Chapeau.Services;
using Chapeau.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chapeau
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IBoothRepo, BoothRepo>();
            builder.Services.AddScoped<IBoothService, BoothService>();
            builder.Services.AddScoped<IStaffRepository,dbUserRepo>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            
            builder.Services.AddScoped<IRestaurantRepo, RestaurantRepo>();
            builder.Services.AddScoped<IRestaurantService, RestaurantSevice>();
            builder.Services.AddSession(options =>
            {
                options.IOTimeout = TimeSpan.FromMinutes(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<IBillRepository, BillRepositoryDB>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<BillService>();
            

            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();
            builder.Services.AddScoped<IBoothRepo, BoothRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
