using Notes_API.Notes;
using Notes_API.DTOs;

namespace Notes_API.Services;

public interface INotesService
{
    Task<IEnumerable<Note>> GetNotes(bool? showArchived, string? searchTerm, int page, int pageSize);
    Task<Note?>  GetNote(int id);
    
    Task<Note> CreateNote(CreateNoteRequest request);
    Task<bool> UpdateNote(int id, UpdateNoteRequest request);
   Task<bool> SoftDeleteNote(int id);
    Task<bool> ToggleArchive(int id);
}