using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;

namespace LogViewer.Library.Module.Common.UI
{
    //Source: https://blogs.msdn.microsoft.com/davidrickard/2010/03/08/saving-window-size-and-location-in-wpf-and-winforms/
    public static class WindowPlacementExtensions
    {
        public static void SetPlacement(this Window window, string placementXml)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            var windowHandle = windowInteropHelper.Handle;
            SetPlacement(windowHandle, placementXml);
        }

        public static string GetPlacement(this Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            var windowHandle = windowInteropHelper.Handle;
            var windowPlacement = GetPlacement(windowHandle);
            return windowPlacement;
        }
        
        private static readonly Encoding Encoding = new UTF8Encoding();
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(WindowPlacement));

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

        private const int ShowNormal = 1;
        private const int ShowMinimized = 2;

        public static void SetPlacement(IntPtr windowHandle, string placementXml)
        {
            if (string.IsNullOrEmpty(placementXml))
            {
                return;
            }

            var xmlBytes = Encoding.GetBytes(placementXml);
            try
            {
                WindowPlacement placement;
                using (var memoryStream = new MemoryStream(xmlBytes))
                {
                    placement = (WindowPlacement)Serializer.Deserialize(memoryStream);
                }

                placement.length = Marshal.SizeOf(typeof(WindowPlacement));
                placement.flags = 0;
                placement.showCmd = (placement.showCmd == ShowMinimized ? ShowNormal : placement.showCmd);
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        public static string GetPlacement(IntPtr windowHandle)
        {
            WindowPlacement placement;
            GetWindowPlacement(windowHandle, out placement);
            using (var memoryStream = new MemoryStream())
            {
                using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    Serializer.Serialize(xmlTextWriter, placement);
                    var xmlBytes = memoryStream.ToArray();
                    return Encoding.GetString(xmlBytes);
                }
            }
        }

        // RECT structure required by WINDOWPLACEMENT structure
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }
        // POINT structure required by WINDOWPLACEMENT structure
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        // WINDOWPLACEMENT stores the position, size, and state of a window
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPlacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point minPosition;
            public Point maxPosition;
            public Rect normalPosition;
        }
    }
}