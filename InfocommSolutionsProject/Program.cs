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
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("InfocommSolutionsProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'InfocommSolutionsProjectContextConnection' not found.");
System.Diagnostics.Debug.WriteLine(connectionString);
builder.Services.AddDbContext<InfocommSolutionsProjectContext>(options =>
    options.UseSqlServer(connectionString)); ;
//sending email :
builder.Services.AddTransient<IEmailSender, EmailSender>();
// end 
builder.Services.AddDefaultIdentity<Accounts>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InfocommSolutionsProjectContext>(); ;

builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));
// testing
//builder.Services.AddAuthorization(options => {
//    //options.AddPolicy("Admin", policy => policy.RequireClaim("IamAdmin"));

//});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

app.Run();