var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/query", async (ctx) =>
{
    await ctx.Response.WriteAsJsonAsync(new { Name = "Faisal", Age = 20 });
});
app.Run("http://localhost:5500");
