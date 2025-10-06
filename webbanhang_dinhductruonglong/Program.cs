using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using webbanhang_dinhductruonglong.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webbanhang_dinhductruonglong.Controllers;
using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Cấu hình chuỗi kết nối với phiên bản MySQL
            

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)) // Thay đổi phiên bản nếu cần
                ));
            builder.Services.AddScoped<IproductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderDetailRepositorycs, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });


            //        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //    };
            //});

            //        // ===== JWT Config =====
            //        var jwtSettings = builder.Configuration.GetSection("Jwt");
            //        builder.Services.AddAuthentication(options =>
            //        {
            //            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        })
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = true,
            //                ValidateAudience = true,
            //                ValidateLifetime = true,
            //                ValidateIssuerSigningKey = true,
            //                ValidIssuer = jwtSettings["Issuer"],
            //                ValidAudience = jwtSettings["Audience"],
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            //            };
            //        });

            builder.Services.AddControllersWithViews();


            builder.Services.AddAuthorization();
            builder.Services.AddRazorPages();
           
            builder.Services.AddControllers();
          
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            
            app.UseSession();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();
            app.UseRouting();
            app.Run();
        }
    }
}
