#if ANDROID

# endif
#if IOS
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
# endif


namespace MatoProductivity.Core.Controls
{
    public partial class WysiwygContentEditor
    {
        public enum StyleType
        {
            underline, italic, bold, backgoundColor, foregroundColor, size
        }
    }
}
