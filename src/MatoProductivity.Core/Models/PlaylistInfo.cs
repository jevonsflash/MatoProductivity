using Abp.AutoMapper;
using MatoProductivity.Core.Models.Entities;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Core.Models
{

    [AutoMapFrom(typeof(Playlist))]
    [AutoMapTo(typeof(Playlist))]
    public class PlaylistInfo : MusicCollectionInfo
    {
        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

        public ImageSource PlaylistArt { get; set; }

    }
}