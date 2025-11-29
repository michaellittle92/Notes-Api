using Microsoft.EntityFrameworkCore;


namespace Notes_API.Data;

public class NotesDbContext : DbContext
{
    public  NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options)
    {}
    public DbSet<Note> Notes { get; set; }
}