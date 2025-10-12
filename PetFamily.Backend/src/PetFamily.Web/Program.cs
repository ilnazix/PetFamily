using PetFamily.Species.Presentation;
using PetFamily.Accounts.Presentation;
using PetFamily.Volunteers.Presentation;
using PetFamily.Volunteers.Presentation.Volunteers;
using PetFamily.Web.Extensions;
using PetFamily.Accounts.Infrastructure.Seeding;
using Serilog;
using Serilog.Events;
using PetFamily.Web.Middlewares;
using PetFamily.VolunteerRequest.Presentation;
using PetFamily.Discussions.Presentation;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddSerilog();

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(SpeciesController).Assembly)
    .AddApplicationPart(typeof(VolunteersController).Assembly)
    .AddApplicationPart(typeof(AccountsController).Assembly);

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
});

builder.Services
    .AddSpeciesModule(builder.Configuration)
    .AddVolunteersModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddVolunteerRequestModule(builder.Configuration)
    .AddDiscussionsModule(builder.Configuration);

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountsSeeder.SeedAsync();

app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


public partial class Program { };