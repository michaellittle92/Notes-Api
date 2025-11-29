
using Microsoft.EntityFrameworkCore;
using Notes_API.Data;
using Notes_API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INotesService, EfCoreNotesService>();

builder.Services.AddDbContext<NotesDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();