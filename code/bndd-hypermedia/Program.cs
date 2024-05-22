var builder = WebApplication.CreateBuilder();
builder.Services.AddMvcCore();

var app = builder.Build();
app.MapControllers();

await app.RunAsync();