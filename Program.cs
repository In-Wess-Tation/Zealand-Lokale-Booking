using Microsoft.AspNetCore.Authentication.Cookies;

namespace Case_2___Zealand_Lokale_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddRazorPages(options =>
            {
                // Angiv hvilke foldere login giver adgang til
                options.Conventions.AuthorizeFolder("/BookingPages");
            });

            builder.Services.AddAuthentication(
            CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Index";
                options.AccessDeniedPath = "/AuthoriaztionDenied";
            });

            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // Aktivér cookie-baseret Authentication
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
