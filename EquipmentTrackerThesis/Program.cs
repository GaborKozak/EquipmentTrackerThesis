using EquipmentTrackerThesis.Data;
using EquipmentTrackerThesis.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EquipmentTrackerThesis.Database.Models;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
//original connection strings for windows
//var masterConnectionString = "Server=localhost; Trusted_connection=True;";
//var appConnectionString = "Server=localhost; DataBase=ManagementSystemDB; Trusted_connection=True;";

//connection strings for MacOS
var masterConnectionString = builder.Configuration["ConnectionStrings:Master"] 
    ?? "Server=localhost; Trusted_connection=True;";
var appConnectionString = builder.Configuration["ConnectionStrings:App"] 
    ?? "Server=localhost; DataBase=ManagementSystemDB; Trusted_connection=True;";

//Login page
builder.Services.AddSingleton<EquipmentTrackerThesis.ILocalStorage, EquipmentTrackerThesis.LocalStorage>();
//Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
//Database create if doesn't exist
var initializer = new DatabaseInitializer(masterConnectionString);
initializer.InitializeDatabase();
//Database connection
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(appConnectionString);
});
builder.Services.AddTransient<DatabaseHandler>();
builder.Services.AddScoped<SignInCheck>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
