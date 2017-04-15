namespace LogViewer.Library.Module.Messages
{
    public class WindowPosition
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public bool Maximized { get; set; }
        public static WindowPosition Default { get; set; } = new WindowPosition()
        {
            Top = 0,
            Left = 0,
            Height = 600,
            Width = 800,
            Maximized = false
        };
    }
}