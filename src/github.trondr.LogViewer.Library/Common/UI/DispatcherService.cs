using System;
using System.Windows;

namespace github.trondr.LogViewer.Library.Common.UI
{
    /// <summary>
    /// Source: http://blogs.msdn.com/b/davidrickard/archive/2010/04/01/using-the-dispatcher-with-mvvm.aspx
    /// </summary>
    public static class DispatcherService
    {
        public static void Invoke(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;
            if(dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }
    }
}
