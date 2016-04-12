namespace github.trondr.LogViewer.Library.Module.Services.WinDebugLog
{
    /// <summary>
    /// Delegate used when firing DebugMonitor.OnOutputDebug event
    /// </summary>
    public delegate void OnOutputDebugStringHandler(int pid, string text);
}
