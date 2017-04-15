using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using ICSharpCode.AvalonEdit.Rendering;

namespace LogViewer.Library.Module.Common.UI
{
    /// <summary>
    /// Source: http://stackoverflow.com/questions/5072761/avalonedit-highlight-current-line-even-when-not-focused
    /// </summary>
    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly CodeTextEditor _editor;

        public HighlightCurrentLineBackgroundRenderer(CodeTextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Caret; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null)
                return;

            if (ViewModelBase.IsInDesignModeStatic)
                return;

            if (_editor.CurrentLine == 0)
                return;

            textView.EnsureVisualLines();
            var currentLine = _editor.Document.GetLineByNumber((int)_editor.CurrentLine -1);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                var brush = new SolidColorBrush(Color.FromArgb(150, 255, 255, 0));
                var newRect = new Rect(new System.Windows.Point(rect.Location.X + textView.ScrollOffset.X, rect.Location.Y), new Size(textView.ActualWidth, rect.Height));
                drawingContext.DrawRectangle(brush, null, newRect);
            }
        }
    }
}