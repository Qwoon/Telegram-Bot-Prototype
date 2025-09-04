using System;
using TelegramBotPrototype.Data;
using TelegramBotPrototype.Http;
using TelegramBotPrototype.SDK;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        builder.Services.AddControllers();
        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.AddSdkServices(builder.Configuration);

        // Health
        builder.Services
            .AddHealthChecks();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapControllers();

        // Error Handling
        var exceptionHandler = new ExceptionHandler
        {
            IncludeExceptionDetails = !builder.Environment.IsProduction() //
        };
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = exceptionHandler.Handle //
        });

        app.MapHealthChecks("/health");

        app.Run();
    }
}