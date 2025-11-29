using System.Runtime.CompilerServices;
using Notes_API;
using Notes_API.Notes;

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

//populate notes list

var notes = new List<Note>
{
    new Note
    {
        Id = 1, Title = "Test Title 01", Content = "Test Content 01", CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now, IsDeleted = false, IsArchived = false
    },
    new Note
    {
        Id = 2, Title = "Test Title 02", Content = "Test Content 02", CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now, IsDeleted = false, IsArchived = false
    },
    new Note
    {
        Id = 3, Title = "Test Title 03", Content = "Test Content 03", CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now, IsDeleted = false, IsArchived = false
    },

};



var notesRoute = app.MapGroup("api/notes"); 


// get notes list
notesRoute.MapGet(string.Empty, (bool? showArchived, string? searchTerm, int page =1, int pageSize = 10) =>
{
    //defualts 
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 10;
    
    int maxPageSize = 100;
    if (pageSize > maxPageSize)  pageSize = maxPageSize;
    
    var activeNotes = notes.Where(n => !n.IsDeleted);

        if (showArchived is false)
        {
            activeNotes = activeNotes.Where(n => !n.IsArchived);
        }
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term =  searchTerm.Trim();
            
            activeNotes = activeNotes.Where(n =>
                n.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                n.Content.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
        
        //order newest first 

            activeNotes = activeNotes.OrderByDescending(n => n.UpdatedAt); 
        
        
        var totalCount = activeNotes.Count();
        var totalPages =  (int)Math.Ceiling((double)totalCount / pageSize);
        
        var items = activeNotes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new
        {
            page, pageSize, totalCount, totalPages, items
        };
        return Results.Ok(result);
});


// get note by id

notesRoute.MapGet("{id:int}", (int id) =>
{
 var note = notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
 if (note is null)
 {
    return Results.NotFound();
 }
 return Results.Ok(note);
});

// post to notes list 

notesRoute.MapPost(string.Empty, (CreateNote note) =>
{
    if (string.IsNullOrEmpty(note.Title))
    {
        return Results.BadRequest("Title is required");
    }
    var newNote = new Note
    {
        Id = notes.Max(n => n.Id) + 1,
        Title = note.Title,
        Content = note.Content,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        IsDeleted = false,
        IsArchived = false
    };
    notes.Add(newNote);
    return Results.Created($"api/notes/{newNote.Id}", newNote);
}
    );

// update note
notesRoute.MapPut("{id:int}", (UpdateNote note, int id) =>
{
    var existingNote = notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);

    if (existingNote is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrEmpty(note.Title))
    {
        return Results.BadRequest("Title is required");
    }
    existingNote.Title  = note.Title;
    existingNote.Content = note.Content;
    existingNote.UpdatedAt = DateTime.Now;
    
    return Results.Ok(existingNote);
});

//soft delete 
notesRoute.MapDelete("{id:int}", (int id) =>
{
    var existingNote = notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
    if (existingNote is null)
    {
        return Results.NotFound();
    }
    existingNote.IsDeleted = true;
    return Results.Ok();
});

//Archive
notesRoute.MapPatch("{id:int}/Archive", (int id) =>
{
    var existingNote = notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
    if (existingNote is null)
    {
        return Results.NotFound();
    }
    existingNote.IsArchived = true;
    return Results.Ok(existingNote);
});

app.UseHttpsRedirection();

app.Run();