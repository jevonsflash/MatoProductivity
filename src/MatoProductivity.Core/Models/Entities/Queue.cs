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
    public class Queue : FullAuditedEntity<long>
    {
        public Queue()
        {

        }
        public Queue(string musicTitle, int rank, long musicInfoId)
        {
            MusicTitle = musicTitle;
            Rank = rank;
            MusicInfoId = musicInfoId;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        public long MusicInfoId { get; set; }

        public int Rank { get; set; }

        public string MusicTitle { get; set; }
    }
}
