using Microsoft.AspNetCore.Mvc;
using Notes_API.DTOs;
using Notes_API.Notes;
using Notes_API.Services;

namespace Notes_API.Controllers;


[ApiController]
[Route("[controller]")]
public class NotesController: ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService ?? throw new ArgumentException(nameof(notesService));
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes(
        bool? showArchived,
        string? searchTerm,
        int page = 1,
        int size = 10)
    {
        var notes = await _notesService.GetNotes(showArchived, searchTerm, page, size);
        var responses = notes.Select(n => new NoteResponse
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt,
            IsArchived = n.IsArchived,

        });
        return Ok(responses);
    }

    [HttpGet("{id}")]
    public IActionResult GetNote(int id)
    {
        var note = _notesService.GetNote(id);
        if (note == null) return NotFound();
        var response = new NoteResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
            IsArchived = note.IsArchived,
        };
        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateNote(CreateNoteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required");
        }
        var note = _notesService.CreateNote(request);

        var response = new NoteResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
            IsArchived = note.IsArchived
        };
        return CreatedAtAction(nameof(GetNote), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateNote(int id, UpdateNoteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required");
        }
        
        var updated = _notesService.UpdateNote(id, request);
        if (!updated) return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteNote(int id)
    {
        var deleted = _notesService.SoftDeleteNote(id);
        {
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

    [HttpPatch("{id:int}/archive")]
    public IActionResult ToggleArchive(int id)
    {
        var ok = _notesService.ToggleArchive(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
