using ReversoAPI;
using TranslatorApp.RequestModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/get-context", async (string text) =>
{
    var reverso = new ReversoClient();

    var synonymData = await reverso.Synonyms.GetAsync(text, Language.English);
    var descriptionData = await reverso.Context.GetAsync(text, Language.English, Language.Russian);
    var synonyms = string.Join(', ', synonymData.Synonyms.Select(x => x?.Value).ToList());
    return Results.Ok(synonyms ?? "No translation found");
})
.WithName("GetContext")
.WithOpenApi();

app.Run();



