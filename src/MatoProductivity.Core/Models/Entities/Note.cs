using Abp.Domain.Entities.Auditing;
using AutoMapper.Configuration.Annotations;
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
        public string Type { get; set; }
        public string Status { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }

        public string PreViewContent { get; set; }

        public bool IsEditable { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

    }
}
