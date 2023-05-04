using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Models.Entities
{
    public class Note : FullAuditedEntity<long>
    {
        public Note()
        {

        }
        public Note(string name, bool isHidden, bool isRemovable)
        {
            Title = name;
            IsHidden = isHidden;
            IsRemovable = isRemovable;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        public ICollection<NoteSegment> NoteSegments { get; set; }

        public string Title { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

    }
}
