using API_UP2.Context;
using API_UP2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<StudentManagementContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection") ??
        "server=127.0.0.1;port=3306;database=StudentManagment;uid=root;pwd=;charset=utf8;Convert Zero Datetime=True",
        new MySqlServerVersion(new Version(8, 0, 0))
    ));

builder.Services.AddScoped<StudentService>();
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