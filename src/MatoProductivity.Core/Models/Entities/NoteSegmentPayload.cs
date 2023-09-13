using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using AutoMapper.Configuration.Annotations;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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



 

        public NoteSegmentPayload(string key, bool value, string valuetype = null)
        {
            this.Key = key;
            this.ValueType = valuetype;
            this.SetStringValue(value.ToString());
        }

        public NoteSegmentPayload(string key, DateTime value, string valuetype = null)
        {
            this.Key = key;
            this.ValueType = valuetype;
            this.SetStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public NoteSegmentPayload(string key, byte[] value, string valuetype = null)
        {
            this.Key = key;
            this.ValueType = valuetype;
            this.Value = value;
        }

        public NoteSegmentPayload(string key, string value, string valuetype = null)
        {
            this.Key = key;
            this.ValueType = valuetype;
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

        [NotMapped]
        public string StringValue => GetStringValue();

        public T GetConcreteValue<T>() where T : struct
        {
            var value = Encoding.UTF8.GetString(Value);
            T result = value.To<T>();
            return result;
        }

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
