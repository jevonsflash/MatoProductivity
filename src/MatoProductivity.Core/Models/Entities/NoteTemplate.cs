using Abp.AutoMapper;
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
    [AutoMap(typeof(Note))]
    public class NoteTemplate : FullAuditedEntity<long>
    {
        public NoteTemplate()
        {

        }
        public NoteTemplate(string name, bool isHidden, bool isRemovable)
        {
            Title = name;
            IsHidden = isHidden;
            IsRemovable = isRemovable;
        }
        [Ignore]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        public ICollection<NoteSegmentTemplate> NoteSegmentTemplates { get; set; }

        public string Title { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

    }
}
