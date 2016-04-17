//Source: Log2Console.Receiver.TcpReceiver

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Logging;
using github.trondr.LogViewer.Library.Module.Services.EventLog;


namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{
    public class TcpLogItemHandler : ILogItemHandler<TcpLogItemConnection>
    {
        public ILogItemConnection Connection { get; set; }
        private ILogItemNotifiable _logItemNotifiable;
        private Socket _socket;
        private readonly ILog4JLogItemParser _log4JLogItemParser;
        private readonly ILog _logger;

        public TcpLogItemHandler(ILog4JLogItemParser log4JLogItemParser, ILog logger)
        {
            _log4JLogItemParser = log4JLogItemParser;
            _logger = logger;
        }


        public void Initialize()
        {
            if (_socket != null) return;

            var connection = GetConnection();


            var ipVersionFamily = connection.IpVersion == IpVersion.Ipv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
            var ipAddress = connection.IpVersion == IpVersion.Ipv6 ? IPAddress.IPv6Any : IPAddress.Any;

            _socket = new Socket(ipVersionFamily, SocketType.Stream, ProtocolType.Tcp) { ExclusiveAddressUse = true };
            _socket.Bind(new IPEndPoint(ipAddress, connection.Port));
            _socket.Listen(100);
            var args = new SocketAsyncEventArgs();
            args.Completed += AcceptAsyncCompleted;
            _socket.AcceptAsync(args);
        }

        public void Terminate()
        {
            if (_socket == null) return;
            _socket.Close();
            _socket.Dispose();
            _socket = null;
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

        ITcpLogItemConnection GetConnection()
        {
            var connection = Connection as ITcpLogItemConnection;
            ValidateConnection(connection);
            return connection;
        }

        private void ValidateConnection(ITcpLogItemConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(connection.HostName)) throw new LogViewerException("HostName has not been initialized.");
            if (connection.Port == 0) throw new LogViewerException("Port has not been initialized.");
            if (connection.IpVersion == IpVersion.Unknown) throw new LogViewerException("IpVersion has not been initialized.");
        }

        void AcceptAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (_socket == null || e.SocketError != SocketError.Success) return;
            new Thread(Start) { IsBackground = true }.Start(e.AcceptSocket);
            e.AcceptSocket = null;
            _socket.AcceptAsync(e);
        }

        void Start(object newSocket)
        {
            try
            {
                var connection = GetConnection();
                using (var socket = (Socket)newSocket)
                {
                    using (var ns = new NetworkStream(socket, FileAccess.Read, false))
                    {
                        while (_socket != null)
                        {                            
                            var logMsg = _log4JLogItemParser.Parse(ns, "TcpLogger");
                            logMsg.Logger = string.Format(":{1}.{0}", logMsg.Logger, connection.Port);
                            _logItemNotifiable?.Notify(logMsg);
                        }
                    }
                }
            }
            catch (IOException ioex)
            {
                _logger.Warn("System.IO.IOException when listning for TCP log items: " + ioex.Message);
            }
            catch (Exception ex)
            {
                //_logger.Warn("System.Exception when listning for TCP log items: " + ex.Message);
            }
        }
    }
}