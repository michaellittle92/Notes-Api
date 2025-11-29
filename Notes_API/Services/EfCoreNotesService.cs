using Microsoft.EntityFrameworkCore;
using Notes_API.Data;
using Notes_API.DTOs;
using Notes_API.Notes;

namespace Notes_API.Services;

public class EfCoreNotesService : INotesService
{
    private readonly NotesDbContext _db;
    
    public EfCoreNotesService(NotesDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<Note>> GetNotes(bool? showArchived, string? searchTerm, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        int maxPageSize = 100;
        if (pageSize > maxPageSize) pageSize = maxPageSize;
        
        var notes = _db.Notes.Where(n => !n.IsArchived);
        
        if (showArchived is false)
        {
            notes = notes.Where(n => !n.IsArchived);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.Trim();
            notes = notes.Where(n => n.Title.Contains(term) || n.Content.Contains(term));
            //Probably should make this case insensitive 
        }
        notes  = notes.OrderByDescending(n => n.UpdatedAt);
        
        return await notes.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Note? GetNote(int id)
    {
        throw new NotImplementedException();
    }

    public Note CreateNote(CreateNoteRequest request)
    {
        throw new NotImplementedException();
    }

    public bool UpdateNote(int id, UpdateNoteRequest request)
    {
        throw new NotImplementedException();
    }

    public bool SoftDeleteNote(int id)
    {
        throw new NotImplementedException();
    }

    public bool ToggleArchive(int id)
    {
        throw new NotImplementedException();
    }
}