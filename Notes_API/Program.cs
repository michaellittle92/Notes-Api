using System.Runtime.CompilerServices;
using Notes_API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var notes = new List<Note>();


//populate notes list
notes.Add(new Note(1, "Test Title 01", "Test Content 01", DateTime.Now, DateTime.Now, false, false));
notes.Add(new Note(2, "Test Title 02", "Test Content 02", DateTime.Now, DateTime.Now, false, false));
notes.Add(new Note(3, "Test Title 03", "Test Content 03", DateTime.Now, DateTime.Now, false, false));

var notesRoute = app.MapGroup("api/notes");


// get notes list
notesRoute.MapGet(String.Empty, () =>
{
    return Results.Ok(notes);
});

app.UseHttpsRedirection();

app.Run();