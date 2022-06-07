using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Areas.Identity.Data;
using InfocommSolutionsProject.Models;
using InfocommSolutionsProject.Services;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity.UI.Services;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);



var services = builder.Services;
var configuration = builder.Configuration;


var connectionString = configuration.GetConnectionString("InfocommSolutionsProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'InfocommSolutionsProjectContextConnection' not found.");
System.Diagnostics.Debug.WriteLine(connectionString);
services.AddDbContext<InfocommSolutionsProjectContext>(options =>
    options.UseSqlServer(connectionString)); ;
//sending email :
services.AddTransient<IEmailSender, EmailSender>();
// end 
services.AddDefaultIdentity<Accounts>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InfocommSolutionsProjectContext>(); ;

services.AddReCaptcha(configuration.GetSection("ReCaptcha"));
services.AddAuthentication()
    .AddGoogle(GoogleOptions => {
        GoogleOptions.ClientId = configuration["Google:ClientId"];
        GoogleOptions.ClientSecret = configuration["Google:ClientSecret"];
      
    });
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireCustomerRole",
        policy => policy.RequireRole("Customer"));
    options.AddPolicy("RequirStaffRole",
       policy => policy.RequireRole("SGAE_STAFF"));
    options.AddPolicy("RequirbothRole",
      policy => policy.RequireRole("SGAE_STAFF" , "Admin"));
  

});

services.AddRazorPages(options =>
{
   
    options.Conventions.AuthorizeFolder("/Supplier", "RequirbothRole");
    options.Conventions.AuthorizeFolder("/SupplierOrders", "RequirbothRole");
    options.Conventions.AuthorizePage("/Product/Delete", "RequireAdministratorRole");
    options.Conventions.AuthorizePage("/Product/Edit", "RequireAdministratorRole");
    options.Conventions.AuthorizePage("/Product/Create", "RequireAdministratorRole");
    options.Conventions.AuthorizeFolder("/Payment");
    //options.Conventions.AuthorizePage("/Contact");
    //options.Conventions.AuthorizeFolder("/Private");
    //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
    //options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
});
// testing
//builder.Services.AddAuthorization(options => {
//    //options.AddPolicy("Admin", policy => policy.RequireClaim("IamAdmin"));

//});

// Add services to the container.
services.AddRazorPages();

// Add SignalR service. IOT related stuff
services.AddSignalR();
services.AddCors(o =>
{
    o.AddPolicy("Everything", p =>
    {
        p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


var app = builder.Build();

IOT _ioTHubEventsReader = new IOT(app.Services);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


System.Diagnostics.Debug.WriteLine("Sussybaka!!!!!");
app.UseHttpsRedirection();
app.UseStaticFiles();
// added new stuff - Zade
app.UseRouting();
app.UseCors("Everything");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<IotSensorWeb.Hubs.SensorHub>("/sensor");
});

app.UseAuthentication(); 
app.UseAuthorization();
app.MapRazorPages();
app.Run();