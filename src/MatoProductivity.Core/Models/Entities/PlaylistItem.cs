﻿using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Models.Entities
{
    public class PlaylistItem : FullAuditedEntity<long>
    {
        public PlaylistItem()
        {

        }
        public PlaylistItem(long playlistId, long musicInfoId, string musicTitle, int rank)
        {
            PlaylistId = playlistId;
            MusicInfoId= musicInfoId;
            MusicTitle = musicTitle;
            Rank = rank;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        public int Rank { get; set; }

        public long PlaylistId { get; set; }
        [ForeignKey("PlaylistId")]
       
        public Playlist Playlist { get; set; }
        public string MusicTitle { get; set; }

        public long MusicInfoId { get; set; }


    }
}
