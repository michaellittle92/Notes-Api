namespace Notes_API.Notes;

public class UpdateNoteRequest
{
    public required string Title { get; set; } = String.Empty;
    public required string? Content { get; set; }
}