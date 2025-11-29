using Notes_API.Notes;
using Notes_API.DTOs;

namespace Notes_API.Services;

public interface INotesService
{
    IEnumerable<Note> GetNotes(bool? showArchived, string? searchTerm, int page, int pageSize);
    Note?  GetNote(int id);
    
    Note CreateNote(CreateNoteRequest request);
    bool UpdateNote(int id, UpdateNoteRequest request);
    bool SoftDeleteNote(int id);
    bool ToggleArchive(int id);
}