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
    public class NoteSegmentPayload : FullAuditedEntity<long>
    {
        public NoteSegmentPayload()
        {

        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [ForeignKey(nameof(NoteSegmentId))]
        public NoteSegment NoteSegment { get; set; }

        public long NoteSegmentId { get; set; }

        public string Key { get; set; }

        public object Value { get; set; }

        public Type ValueType { get; set; }



    }
}
