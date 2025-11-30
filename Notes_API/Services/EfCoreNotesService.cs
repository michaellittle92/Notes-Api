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

    public async Task<Note?> GetNote(int id)
    {
        return await _db.Notes
            .Where(n => !n.IsDeleted && n.Id == id)
            .FirstOrDefaultAsync();
    }


    public async Task<Note> CreateNote(CreateNoteRequest request)
    {
        var now = DateTime.Now;

        var note = new Note
        {
            Title = request.Title.Trim(),
            Content = request.Content,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false,
            IsArchived = false

        };
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return note;
    }

    public async Task<bool> UpdateNote(int id, UpdateNoteRequest request)
    {
       var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
       if (note is null) return false;
       
       note.Title = request.Title.Trim();
       note.Content = request.Content.Trim() ??  string.Empty;
       note.UpdatedAt = DateTime.Now;
       
       await _db.SaveChangesAsync();
       return true;
    }

    public async Task<bool> SoftDeleteNote(int id)
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
        if (note is null) return false;
        
        note.IsDeleted = true;
        note.UpdatedAt = DateTime.Now;
        
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleArchive(int id)
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id && !n.IsArchived);
        if (note is null) return false; 
        note.IsArchived = !note.IsArchived;
        note.UpdatedAt = DateTime.Now;
        
        await _db.SaveChangesAsync();
        return true;
    }
}