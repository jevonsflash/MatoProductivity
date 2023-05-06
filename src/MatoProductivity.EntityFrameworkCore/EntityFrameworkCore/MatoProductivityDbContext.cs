using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MatoProductivity.Core.Theme;
using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.EntityFrameworkCore
{
    public class MatoProductivityDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public DbSet<Note> Note { get; set; }
        public DbSet<NoteGroup> NoteGroup { get; set; }
        public DbSet<NoteSegment> NoteSegment { get; set; }
        public DbSet<NoteSegmentStore> NoteSegmentStore { get; set; }
        public DbSet<NoteSegmentPayload> NoteSegmentPayload { get; set; }
        public DbSet<NoteTemplate> NoteTemplate { get; set; }
        public DbSet<NoteSegmentTemplate> NoteSegmentTemplate { get; set; }
        public DbSet<NoteSegmentTemplatePayload> NoteSegmentTemplatePayload { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public MatoProductivityDbContext(DbContextOptions<MatoProductivityDbContext> options) 
            : base(options)
        {

        }
    }
}
