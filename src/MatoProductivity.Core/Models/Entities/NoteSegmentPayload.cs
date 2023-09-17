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


        public NoteSegmentPayload(string key, object value, string valuetype = null)
        {
            if (value is string)
            {
                this.SetStringValue((value as string).ToString());
            }
            else if (value is byte[])
            {
                this.Value = value as byte[];
            }
            else if (value is DateTime)
            {
                this.SetStringValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                this.SetStringValue(value.ToString());
            }
            this.Key = key;
            this.ValueType = valuetype;

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
