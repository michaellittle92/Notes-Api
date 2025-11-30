namespace Notes_API.DTOs;

public class CreateNoteRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
}