using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MatoProductivity.Core.Theme;
using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.EntityFrameworkCore
{
    public class MatoProductivityDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public DbSet<Queue> Queue { get; set; }
        public DbSet<NoteGroup> Playlist { get; set; }
        public DbSet<PlaylistItem> PlaylistItem { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public MatoProductivityDbContext(DbContextOptions<MatoProductivityDbContext> options) 
            : base(options)
        {

        }
    }
}
