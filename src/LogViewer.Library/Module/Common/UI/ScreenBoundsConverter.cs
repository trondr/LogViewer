using System.Windows;
using System.Windows.Media;
using System.Drawing;


namespace LogViewer.Library.Module.Common.UI
{
    /// <summary>
    /// Source: http://stackoverflow.com/questions/2633759/ensuring-wpf-window-is-inside-screen-bounds
    /// </summary>
    public class ScreenBoundsConverter
    {
        private readonly Matrix _transform;

        public ScreenBoundsConverter(Visual visual)
        {
            var presentationSource = PresentationSource.FromVisual(visual);
            if (presentationSource?.CompositionTarget != null)
                _transform = presentationSource.CompositionTarget.TransformFromDevice;
        }

        public Rect ConvertBounds(Rectangle bounds)
        {
            var result = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            result.Transform(_transform);
            return result;
        }
    }
}
