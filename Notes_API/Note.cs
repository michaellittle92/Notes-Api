namespace Notes_API;

public class Note
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsArchived { get; set; }
    public bool IsDeleted { get; set; }

    public Note(int id, string title, string content, DateTime createdAt, DateTime updatedAt, bool isArchived,
        bool isDeleted)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsArchived = isArchived;
        IsDeleted = isDeleted;
    }
}

