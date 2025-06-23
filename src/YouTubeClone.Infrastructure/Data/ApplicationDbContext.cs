using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouTubeClone.Shared.Models;
using YouTubeClone.Infrastructure.Identity;

namespace YouTubeClone.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<VideoLike> VideoLikes { get; set; } = null!;
    public DbSet<CommentLike> CommentLikes { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Video configuration
        builder.Entity<Video>()
            .HasOne(v => v.User)
            .WithMany(u => u.Videos)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore QualityUrls as it's not mapped to database
        builder.Entity<Video>()
            .Ignore(v => v.QualityUrls);

        // Comment configuration
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Video)
            .WithMany(v => v.Comments)
            .HasForeignKey(c => c.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        // VideoLike configuration
        builder.Entity<VideoLike>()
            .HasOne(vl => vl.User)
            .WithMany(u => u.VideoLikes)
            .HasForeignKey(vl => vl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<VideoLike>()
            .HasOne(vl => vl.Video)
            .WithMany(v => v.Likes)
            .HasForeignKey(vl => vl.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        // CommentLike configuration
        builder.Entity<CommentLike>()
            .HasOne(cl => cl.User)
            .WithMany()
            .HasForeignKey(cl => cl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CommentLike>()
            .HasOne(cl => cl.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Subscription configuration
        builder.Entity<Subscription>()
            .HasOne(s => s.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.SubscriberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Subscription>()
            .HasOne(s => s.Creator)
            .WithMany(u => u.Subscribers)
            .HasForeignKey(s => s.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Playlist configuration
        builder.Entity<Playlist>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Remove many-to-many relationship for now to avoid cascade issues
        builder.Entity<Playlist>()
            .Ignore(p => p.Videos);
        
        builder.Entity<Video>()
            .Ignore(v => v.Playlists);
    }
}
