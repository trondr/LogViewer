﻿using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using github.trondr.LogViewer.Library.Module.Model;
using log4net.Appender;
using log4net.Core;

namespace github.trondr.LogViewer.Library.Module.Services.RemotingLog
{
    [Serializable]
    public class RemotingLogItemHandler : MarshalByRefObject, ILogItemHandler<RemotingLogItemConnection>, RemotingAppender.IRemoteLoggingSink, ISerializable
    {
        private const string RemotingReceiverChannelName = "RemotingReceiverChannel";
        private readonly ILogItemFactory _logItemFactory;
        private readonly ILogLevelProvider _logLevelProvider;
        private ILogItemNotifiable _logItemNotifiable;
        private readonly object _sync = new object();
        private IChannel _channel;

        public RemotingLogItemHandler(ILogItemFactory logItemFactory, ILogLevelProvider logLevelProvider)
        {
            _logItemFactory = logItemFactory;
            _logLevelProvider = logLevelProvider;
        }

        public void Initialize()
        {
           // Channel already open?
			_channel = ChannelServices.GetChannel(RemotingReceiverChannelName);
            var connection = GetConnection();

			if (_channel == null)
			{
				// Allow clients to receive complete Remoting exception information
				if (RemotingConfiguration.CustomErrorsEnabled(true))
					RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;

				// Create TCP Channel
				try
				{
					BinaryClientFormatterSinkProvider clientProvider = null;
				    var serverProvider = new BinaryServerFormatterSinkProvider
				    {
				        TypeFilterLevel = TypeFilterLevel.Full
				    };
                    
				    IDictionary props = new Hashtable();
					props["port"] = connection.Port.ToString();
					props["name"] = RemotingReceiverChannelName;
					props["typeFilterLevel"] = TypeFilterLevel.Full;
					_channel = new TcpChannel(props, clientProvider, serverProvider);                    

					ChannelServices.RegisterChannel(_channel, false);
				}
				catch (Exception ex)
				{
					throw new Exception("Remoting TCP Channel Initialization failed", ex);
				}
			}

			var serverType = RemotingServices.GetServerTypeForUri(connection.SinkName);
			if ((serverType == null) || (serverType != typeof(RemotingAppender.IRemoteLoggingSink)))
			{
				// Marshal Receiver
				try
				{
					RemotingServices.Marshal(this, connection.SinkName, typeof(RemotingAppender.IRemoteLoggingSink));
				}
				catch (Exception ex)
				{
					throw new Exception("Remoting Marshal failed", ex);
				}
			}         
        }

        public void Terminate()
        {
            if (_channel != null)
				ChannelServices.UnregisterChannel(_channel);
			_channel = null;
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            lock(_sync)
            {
                _logItemNotifiable = logItemNotifiable;
            }
        }

        public void Detach()
        {
            lock(_sync)
            {
                _logItemNotifiable = null;
            }
        }

        public bool ShowFromBeginning { get; set; }

        public ILogItemConnection Connection { get; set; }

        private IRemotingLogItemConnection GetConnection()
        {
            var connection = Connection as IRemotingLogItemConnection;
            ValidateEventLogConnection(connection);
            return connection;
        }

        private void ValidateEventLogConnection(IRemotingLogItemConnection remotingLogConnection)
        {
            if (remotingLogConnection == null) throw new ArgumentNullException(nameof(remotingLogConnection));
            if (string.IsNullOrEmpty(remotingLogConnection.SinkName)) throw new LogViewerException("SinkName has not been initialized.");
            if (remotingLogConnection.Port == 0) throw new LogViewerException("Port has not been initialized.");            
        }

        public void LogEvents(LoggingEvent[] events)
        {
            if( (events == null) || (events.Length == 0) || _logItemNotifiable == null)
                return;

            foreach (var loggingEvent in events)
            {
                var logItem = GetLogItem(loggingEvent);
                _logItemNotifiable.Notify(logItem);
            }
        }

        private LogItem GetLogItem(LoggingEvent loggingEvent)
        {
            var loggerName = GetLoggerName(loggingEvent);
            var message = loggingEvent.RenderedMessage;
            var timeStamp = loggingEvent.TimeStamp;
            var logLevel = _logLevelProvider.GetLogLevel(loggingEvent.Level.Value);
            var threadId = loggingEvent.ThreadName;
            var exceptionString = loggingEvent.GetExceptionString();
            var logItem = _logItemFactory.GetLogItem(timeStamp, logLevel, loggerName, threadId, message, exceptionString);
            foreach (DictionaryEntry property in loggingEvent.Properties)
            {
                if ((property.Key == null) || (property.Value == null))
                    continue;

                logItem.Properties.Add(property.Key.ToString(),property.Value.ToString());
            }            
            return logItem;
        }

        private string GetLoggerName(LoggingEvent loggingEvent)
        {
            var loggerName = loggingEvent.Properties.Contains(LoggingEvent.HostNameProperty)
        	                    	? string.Format("[Host: {0}].{1}", loggingEvent.Properties[LoggingEvent.HostNameProperty], loggingEvent.LoggerName)
        	                    	: loggingEvent.LoggerName;
            return loggerName;
        }

        public RemotingLogItemHandler(SerializationInfo info, StreamingContext context)
        {
            Connection = (RemotingLogItemConnection)info.GetValue("Connection", typeof(RemotingLogItemConnection));            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var connection = GetConnection();
            info.AddValue("Connection", connection);
        }
    }
}