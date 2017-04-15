﻿//Source: Log2Console.Receiver.TcpReceiver

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Library.Module.Services.UdpLog
{
    public class UdpLogItemHandler : ILogItemHandler<UdpLogItemConnection>
    {
        public ILogItemConnection Connection { get; set; }
        private ILogItemNotifiable _logItemNotifiable;
        private readonly ILog4JLogItemParser _log4JLogItemParser;
        private Thread _worker;
        private IPEndPoint _remoteEndPoint;
        private UdpClient _udpClient;

        public UdpLogItemHandler(ILog4JLogItemParser log4JLogItemParser)
        {
            _log4JLogItemParser = log4JLogItemParser;
        }


        public void Initialize()
        {
            if (_worker != null && _worker.IsAlive)
                return;
            var connection = GetConnection();
            var ipVersionFamily = connection.IpVersion == IpVersion.Ipv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
            
            // Init connection here, before starting the thread, to know the status now
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _udpClient = ipVersionFamily == AddressFamily.InterNetworkV6 ? new UdpClient(connection.Port, AddressFamily.InterNetworkV6) : new UdpClient(connection.Port);

            if (connection.MultiCastAddress != null)
                _udpClient.JoinMulticastGroup(connection.MultiCastAddress);

            // We need a working thread
            _worker = new Thread(Start) {IsBackground = true};
            _worker.Start();
        }

        public void Terminate()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null;

                _remoteEndPoint = null;
            }

            if ((_worker != null) && _worker.IsAlive)
                _worker.Abort();
            _worker = null;
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            _logItemNotifiable = logItemNotifiable;
        }

        public void Detach()
        {
            _logItemNotifiable = null;
        }

        public bool ShowFromBeginning { get; set; }

        public string DefaultLoggerName { get; set; }

        IUdpLogItemConnection GetConnection()
        {
            var connection = Connection as IUdpLogItemConnection;
            ValidateConnection(connection);
            return connection;
        }

        private void ValidateConnection(IUdpLogItemConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(connection.HostName)) throw new LogViewerException("HostName has not been initialized.");
            if (connection.Port == 0) throw new LogViewerException("Port has not been initialized.");
            if (connection.IpVersion == IpVersion.Unknown) throw new LogViewerException("IpVersion has not been initialized.");
        }

        private void Start()
        {
            while ((_udpClient != null) && (_remoteEndPoint != null))
            {
                try
                {
                    var buffer = _udpClient.Receive(ref _remoteEndPoint);
                    var loggingEvent = System.Text.Encoding.UTF8.GetString(buffer);

                    Console.WriteLine(loggingEvent);

                    if (_logItemNotifiable == null)
                        continue;

                    var connection = GetConnection();
                    var logMsg = _log4JLogItemParser.Parse(loggingEvent, "UdpLogger");
                    logMsg.Logger = string.Format(":{1}.{0}", logMsg.Logger, connection.Port);
                    _logItemNotifiable?.Notify(logMsg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }
            }
        }
    }
}