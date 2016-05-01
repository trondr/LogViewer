﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.ManualTests.FileLogTests
{
    [TestFixture(Category = "ManualTests")]
    public class FileLogManualTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, RequiresSTA]
        public void FileLogTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var container = bootStrapper.Container;
                var openLogProvider = container.Resolve<IOpenLogCommandProvider>();
                var logGenerator = new TestFileLogGenerator(@"%public%\Logs\FileLogTests-%COMPUTERNAME%-%USERNAME%.log");
                StartFileTestLogger(logGenerator);
                openLogProvider.OpenLogs(new string[] {@"file://%public%\Logs\FileLogTests-%COMPUTERNAME%-%USERNAME%.log"});
                StoppTcpTestLogger(logGenerator);
            }
        }

        [Test]
        public void FileFlushTest_FlushBug()
        {
            var testMessage = new string('T', 1024);
            var logFile = Environment.ExpandEnvironmentVariables(@"%public%\Logs\FileLogTests-NotFlushedBug-%COMPUTERNAME%-%USERNAME%-CustomLog.log");
            using (var sw = new StreamWriter(logFile))
            {
                sw.AutoFlush = true; // Bug: The file is not actually flushed to disk.
                while (true)
                {
                    sw.WriteLine(testMessage);
                    sw.Flush(); // Bug: This does not work either. Watch in Windows Explorer and see that file size is NOT changing. F5 is required to trigger flush to disk.
                    Thread.Sleep(500);
                }                
            }
        }

        [Test]
        public void FileFlushTest_FlushBugWorkaround()
        {
            var testMessage = new string('T', 1024);
            var logFile = Environment.ExpandEnvironmentVariables(@"%public%\Logs\FileLogTests-FlushedWorkaround-%COMPUTERNAME%-%USERNAME%-CustomLog.log");
            using (var sw = new StreamWriter(logFile))
            {
                while (true)
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(testMessage);
                    NativeWin32Flush(sw); //Now the file is flushed to disk! Watch in Windows Explorer and see that file size is changing without pressing F5.
                    Thread.Sleep(500);
                }                
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [ResourceExposure(ResourceScope.None)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlushFileBuffers(SafeFileHandle hFile);

        private void NativeWin32Flush(StreamWriter sw)
        {
            var fs = sw.BaseStream as FileStream;
            if(fs == null) return;
            var flushed = FlushFileBuffers(fs.SafeFileHandle);
            if (flushed) return;
            var error = Marshal.GetLastWin32Error();
            throw new Win32Exception(error);
        }

        private static void StoppTcpTestLogger(TestFileLogGenerator logGenerator)
        {
            logGenerator.Stop();
            Thread.Sleep(1000);
        }

        private void StartFileTestLogger(TestFileLogGenerator logGenerator)
        {
            if (logGenerator == null) throw new ArgumentNullException(nameof(logGenerator));
            var threadStart = new ThreadStart(logGenerator.Start);
            var thread = new Thread(threadStart);
            thread.Start();
        }        
    }
}