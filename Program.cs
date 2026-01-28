using API_UP2.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentManagementContext>(options =>
            options.UseMySql("server=127.0.0.1;" +
            "uid=root;" +
            "pwd=;" +
            "database=TaskManager",
            new MySqlServerVersion(new Version(8, 0, 11))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Student Management API",
        Description = "API для управления студентами"
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();