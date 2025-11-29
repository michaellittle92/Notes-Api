namespace Notes_API.DTOs;

public class CreateNoteRequest
{
    public required string Title { get; set; } = String.Empty;
    public required string? Content { get; set; }
}