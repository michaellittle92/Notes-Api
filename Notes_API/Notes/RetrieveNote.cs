namespace Notes_API.Notes;

public class RetrieveNote
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsArchived { get; set; }
    public bool IsDeleted { get; set; }
    
}