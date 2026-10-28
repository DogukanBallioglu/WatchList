using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WatchList.Api.Models;

namespace WatchList.Api.Data;

public class WatchListContext : DbContext
{
    public WatchListContext(DbContextOptions<WatchListContext> options) : base(options)
    {
        
    }

    public DbSet<MediaItem> Movies => Set<MediaItem>();
}