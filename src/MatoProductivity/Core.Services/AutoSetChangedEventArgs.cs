using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class AutoSetChangedEventArgs: EventArgs
    {
        public bool IsAutoSet { get; set; }

        public AutoSetChangedEventArgs(bool isAutoSet)
        {
            this.IsAutoSet= isAutoSet;
        }
    }
}
