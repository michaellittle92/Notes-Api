using Notes_API.Notes;
using Notes_API.DTOs;

namespace Notes_API.Services;

public class InMemoryNotesService : INotesService
{
    private readonly List<Note> _notes = new();
    private int _nextId = 1;

    public InMemoryNotesService()
    {
        var now = DateTime.Now;
        _notes.Add(new Note
        {
            Id = _nextId++,
            Title = "Test Title 01",
            Content = "Test Content 01",
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false,
            IsArchived = false
        });

        _notes.Add(new Note
        {
            Id = _nextId++,
            Title = "Test Title 02",
            Content = "Test Content 02",
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false,
            IsArchived = false
        });

        _notes.Add(new Note
            {
                Id = _nextId++,
                Title = "Test Title 03",
                Content = "Test Content 03",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
                IsArchived = false
            }
        );
    }

    public async Task<IEnumerable<Note>> GetNotes(bool? showArchived, string? searchTerm, int page = 1, int pageSize = 10)
    {
        //defualts 
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        int maxPageSize = 100;
        if (pageSize > maxPageSize) pageSize = maxPageSize;

        var activeNotes = _notes.Where(n => !n.IsDeleted);

        if (showArchived is false)
        {
            activeNotes = activeNotes.Where(n => !n.IsArchived);
        }

        //search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.Trim();

            activeNotes = activeNotes.Where(n =>
                n.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                n.Content.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        //order newest first 

        activeNotes = activeNotes.OrderByDescending(n => n.UpdatedAt);

        //pagination
        activeNotes = activeNotes.Skip((page - 1) * pageSize).Take(pageSize);
        return activeNotes.ToList();
    }

    public async Task<Note?> GetNote(int id)
    { 
        return _notes.FirstOrDefault(n => n.Id == id && n.IsDeleted == false);
    }

    public async Task<Note> CreateNote(CreateNoteRequest request)
    {
        var now = DateTime.Now;

        var note = new Note
        {
            Id = _nextId++,
            Title = request.Title,
            Content = request.Content,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false,
            IsArchived = false

        };
        _notes.Add(note);
        return note;
    }

    public async Task <bool> UpdateNote(int id, UpdateNoteRequest request)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
        if (note is null) return false;

        note.Title = request.Title;
        note.Content = request.Content ?? string.Empty;
        note.UpdatedAt = DateTime.Now;

        return true;
    }
    

    public async Task<bool> SoftDeleteNote(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
        if (note is null) return false;

        note.IsDeleted = true;
        note.UpdatedAt = DateTime.Now;
        return true;
    }

    public async Task<bool> ToggleArchive(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
        if (note is null) return false;
        
        note.IsArchived = !note.IsArchived;
        note.UpdatedAt = DateTime.Now;
        return true;
    }
}
    