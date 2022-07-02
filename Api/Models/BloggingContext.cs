using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class BloggingContext : DbContext{
    public DbSet<Blog> Blogs { get; set; }

    public string DbPath { get; }
    public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) {
        
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }
    
    
}


public class Blog {
    public int BlogId { get; set; }
    public string Url { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Post {
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
