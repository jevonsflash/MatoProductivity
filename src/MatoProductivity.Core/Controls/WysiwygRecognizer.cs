using Lession2.WysiwygRecognizer;
using Microsoft.Maui.Platform;

namespace Lession2.WysiwygRecognizer
{
    public partial class WysiwygRecognizer: IDisposable
    {
        public event EventHandler<WysiwygActionEventArgs> OnWysiwygActionInvoked;
        public partial void Dispose();
    }
}
