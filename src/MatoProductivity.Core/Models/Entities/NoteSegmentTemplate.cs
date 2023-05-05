using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
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
    [AutoMap(typeof(NoteSegment))]
    public class NoteSegmentTemplate : FullAuditedEntity<long>
    {
        public NoteSegmentTemplate()
        {

        }


        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [ForeignKey(nameof(NoteTemplateId))]
        public NoteTemplate NoteTemplate { get; set; }

        public ICollection<NoteSegmentTemplatePayload> NoteSegmentTemplatePayloads { get; set; }

        public long NoteTemplateId { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Rank { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }


        public void SetNoteSegmentTemplatePayloads(NoteSegmentTemplatePayload noteSegmentPayload)
        {
            if (NoteSegmentTemplatePayloads != null)
            {
                var currentPayload = NoteSegmentTemplatePayloads.FirstOrDefault(c => c.Key == noteSegmentPayload.Key);
                if (currentPayload != null)
                {
                    NoteSegmentTemplatePayloads.Remove(currentPayload);
                }
                NoteSegmentTemplatePayloads.Add(noteSegmentPayload);
            }
        }

        public NoteSegmentTemplatePayload GetOrSetNoteSegmentTemplatePayloads(string key, NoteSegmentTemplatePayload noteSegmentPayload)
        {
            if (NoteSegmentTemplatePayloads != null)
            {
                var currentPayload = NoteSegmentTemplatePayloads.FirstOrDefault(c => c.Key == key);
                if (currentPayload != null)
                {
                    return currentPayload;
                }
                if (noteSegmentPayload != null)
                {
                    NoteSegmentTemplatePayloads.Add(noteSegmentPayload);
                }
                return noteSegmentPayload;
            }
            return noteSegmentPayload;
        }

    }
}
