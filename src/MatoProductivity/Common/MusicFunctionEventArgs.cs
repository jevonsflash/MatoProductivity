using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatoProductivity.Core.Models;
using MatoProductivity.Infrastructure.Common;

namespace MatoProductivity.Common
{
    public class MusicFunctionEventArgs : EventArgs
    {
        public MusicFunctionEventArgs(IBasicInfo musicInfo, MenuCellInfo menuCellInfo)
        {
            this.MusicInfo = musicInfo;
            this.MenuCellInfo = menuCellInfo;
        }
        public IBasicInfo MusicInfo { get; set; }
        public MenuCellInfo MenuCellInfo { get; set; }
    }
}
