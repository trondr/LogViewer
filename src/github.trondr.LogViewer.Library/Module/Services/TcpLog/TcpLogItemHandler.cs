using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Module.Model;
using github.trondr.LogViewer.Library.Module.Services.EventLog;
using LogLevel = github.trondr.LogViewer.Library.Module.Model.LogLevel;

namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{
    public class TcpLogItemHandler : ILogItemHandler<TcpLogItemConnection>
    {
        public ILogItemConnection Connection { get; set; }
        private ILogItemNotifiable _logItemNotifiable;
        private static readonly List<LogLevel> LogLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
        private static Random _random = new Random();
        private Socket _socket;


        public void Initialize()
        {
           if (_socket != null) return;

           var connection = GetConnection();


           var ipVersionFamily = connection.IpVersion == IpVersion.Ipv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
            var ipAddress = connection.IpVersion == IpVersion.Ipv6 ? IPAddress.IPv6Any : IPAddress.Any;

      _socket = new Socket(ipVersionFamily, SocketType.Stream, ProtocolType.Tcp);
      _socket.ExclusiveAddressUse = true;
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
        using (var socket = (Socket)newSocket)
        using (var ns = new NetworkStream(socket, FileAccess.Read, false))
          while (_socket != null)
          {
                        ToDo.Implement(ToDoPriority.Critical, "trondr","Implement the rest of the Start method");
            //var logMsg = ReceiverUtils.ParseLog4JXmlLogEvent(ns, "TcpLogger");
            //logMsg.LoggerName = string.Format(":{1}.{0}", logMsg.LoggerName, _port);

            //if (Notifiable != null)
            //  Notifiable.Notify(logMsg);
          }
      }
      catch (IOException)
      {
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }
    }
}