using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Theme
{
    public class Theme : FullAuditedEntity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }

        public bool IsSel { get; set; }
        public string ColorA { get; set; }
        public string ColorB { get; set; }
        public string ColorC { get; set; }
    }
}
