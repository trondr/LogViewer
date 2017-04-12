using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ICSharpCode.AvalonEdit;
using LogViewer.Library.Annotations;

namespace LogViewer.Library.Module.Common.UI
{
    public class CodeTextEditor : TextEditor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static readonly DependencyProperty Text2Property = DependencyProperty.Register(
            "Text2", typeof(string), typeof(CodeTextEditor), new PropertyMetadata(default(string), (o, args) =>
            {
                var target = (CodeTextEditor) o;
                target.Text = (string)args.NewValue;

            }));

        public string Text2
        {
            get { return (string) GetValue(Text2Property); }
            set { SetValue(Text2Property, value); OnPropertyChanged();}
        }
    }
}
