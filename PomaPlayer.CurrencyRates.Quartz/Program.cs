using Microsoft.AspNetCore.HttpLogging;
using PomaPlayer.CurrencyRates.Quartz.Extensioncs;
using PomaPlayer.CurrencyRates.Quartz.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddQuartzServices(builder.Configuration);

builder.Services.AddHttpLogging(o => o = new HttpLoggingOptions());

var app = builder.Build();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quartz");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
