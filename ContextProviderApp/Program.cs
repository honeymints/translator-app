using ContextProviderApp.Interfaces;
using ContextProviderApp.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<DictionaryService>();
        builder.Services.AddTransient<TelegramClientService>();

        builder.Services.AddHttpClient("Dictionary", client =>
        {
            client.BaseAddress = new Uri("https://api.dictionaryapi.dev/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();

        app.MapGet("/get-context/{text}", async (DictionaryService dictionaryService, string text) =>
        {
            var dictionary = await dictionaryService.GetDictionaryAsync(text);
            return Results.Ok(dictionary);
        });


        app.Run();
    }
}