using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ICSharpCode.AvalonEdit;
using LogViewer.Library.Properties;

namespace LogViewer.Library.Module.Common.UI
{
    public class CodeTextEditor : TextEditor, INotifyPropertyChanged
    {
        public CodeTextEditor()
        {            
            this.TextArea.TextView.BackgroundRenderers.Add(
                new HighlightCurrentLineBackgroundRenderer(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static readonly DependencyProperty Text2Property = DependencyProperty.Register(
            "Text2", typeof(string), typeof(CodeTextEditor), new PropertyMetadata(default(string), (o, args) =>
            {
                var target = (CodeTextEditor)o;
                target.Text = (string)args.NewValue;
                target.ScrollToLine((int)target.CurrentLine);
            }));

        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.Register(
            "CurrentLine", typeof(uint), typeof(CodeTextEditor), new PropertyMetadata(default(uint), (o, args) =>
            {
                var target = (CodeTextEditor)o;
                target.ScrollToLine((int)target.CurrentLine);
            }));

        public uint CurrentLine
        {
            get { return (uint)GetValue(CurrentLineProperty); }
            set { SetValue(CurrentLineProperty, value); }
        }
    }
}
