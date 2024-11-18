using Web.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddMvc();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSignalR();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddSingleton<IMessageGenerator, MessageGenerator>();

var app = builder.Build();
app.MapRazorPages();
app.MapControllers();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();
