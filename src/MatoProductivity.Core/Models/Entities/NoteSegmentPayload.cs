using Abp.Domain.Entities.Auditing;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MatoProductivity.Core.Models.Entities
{
    public class NoteSegmentPayload : FullAuditedEntity<long>
    {
        public NoteSegmentPayload()
        {

        }

        public NoteSegmentPayload(string key, string value)
        {
            this.Key = key;
            this.SetStringValue(value);
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [ForeignKey(nameof(NoteSegmentId))]
        public NoteSegment NoteSegment { get; set; }

        public long NoteSegmentId { get; set; }

        public string Key { get; set; }

        public byte[] Value { get; set; }

        public string ValueType { get; set; }

        public string GetStringValue()
        {
            var value = Encoding.UTF8.GetString(Value);
            return value;
        }

        public void SetStringValue(string value)
        {
            this.Value = Encoding.UTF8.GetBytes(value);
        }
    }
}
