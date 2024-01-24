using Abp.AutoMapper;
using Abp.Domain.Entities;
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
    public class Setting : Entity<string>
    {
        public Setting(string id, string value)
        {
            Id=id;
            Value=value;
        }

        [Key]
        public override string Id { get; set; }
        public string Value { get; set; }

    }
}
