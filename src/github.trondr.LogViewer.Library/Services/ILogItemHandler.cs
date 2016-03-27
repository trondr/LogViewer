﻿namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemHandler<T> : ILogItemHandler where T : ILogItemConnection
    {
        
    }

    public interface ILogItemHandler
    {
        ILogItemConnection Connection { get; set; }

        void Initialize();

        void Terminate();

        void Attach(ILogItemNotifiable logItemNotifiable);

        void Detach();

        bool ShowFromBeginning { get; set; }
    }    
}