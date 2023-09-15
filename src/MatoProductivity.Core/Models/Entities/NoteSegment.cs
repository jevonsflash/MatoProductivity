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
    public class NoteSegment : FullAuditedEntity<long>
    {
        public NoteSegment()
        {

        }



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [ForeignKey(nameof(NoteId))]
        public Note Note { get; set; }

        public ICollection<NoteSegmentPayload> NoteSegmentPayloads { get; set; }

        public long NoteId { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Rank { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }


        public NoteSegmentPayload GetNoteSegmentPayload(string key)
        {
            if (NoteSegmentPayloads != null)
            {
                return NoteSegmentPayloads.FirstOrDefault(c => c.Key==key);
            }
            return default;
        }

        public void RemoveNoteSegmentPayloads(string noteSegmentPayloadKey)
        {
            if (NoteSegmentPayloads != null)
            {
                var currentPayload = NoteSegmentPayloads.FirstOrDefault(c => c.Key == noteSegmentPayloadKey);
                if (currentPayload != null)
                {
                    NoteSegmentPayloads.Remove(currentPayload);
                }
            }
        }

        public void RemoveNoteSegmentPayloads(NoteSegmentPayload noteSegmentPayload)
        {
            if (NoteSegmentPayloads != null)
            {
                var currentPayload = NoteSegmentPayloads.FirstOrDefault(c => c.Key == noteSegmentPayload.Key);
                if (currentPayload != null)
                {
                    NoteSegmentPayloads.Remove(currentPayload);
                }
            }
        }


        public void SetNoteSegmentPayloads(NoteSegmentPayload noteSegmentPayload)
        {
            if (NoteSegmentPayloads != null)
            {
                var currentPayload = NoteSegmentPayloads.FirstOrDefault(c => c.Key == noteSegmentPayload.Key);
                if (currentPayload != null)
                {
                    NoteSegmentPayloads.Remove(currentPayload);
                }
                if (!this.IsTransient())
                {
                    noteSegmentPayload.NoteSegmentId=this.Id;
                }
                NoteSegmentPayloads.Add(noteSegmentPayload);
            }
        }

        public NoteSegmentPayload GetOrSetNoteSegmentPayloads(string key, NoteSegmentPayload noteSegmentPayload)
        {
            if (NoteSegmentPayloads != null)
            {
                var currentPayload = NoteSegmentPayloads.FirstOrDefault(c => c.Key == key);
                if (currentPayload != null)
                {
                    return currentPayload;
                }
                if (noteSegmentPayload != null)
                {
                    if (!this.IsTransient())
                    {
                        noteSegmentPayload.NoteSegmentId=this.Id;
                    }
                    NoteSegmentPayloads.Add(noteSegmentPayload);
                }
                return noteSegmentPayload;
            }
            return noteSegmentPayload;
        }

    }
}
