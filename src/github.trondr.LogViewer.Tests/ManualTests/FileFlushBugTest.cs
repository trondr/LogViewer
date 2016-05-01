using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using github.trondr.LogViewer.Tests.ManualTests.FileLogTests;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.ManualTests
{
    [TestFixture(Category = "ManualTests")]
    public class FileFlushBugTest
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

        [Test]
        public void FileFlushTest_FlushBug()
        {
            var messageLength = 1024;
            var testMessage = new string('T', messageLength);
            var logFile = Environment.ExpandEnvironmentVariables(@"%public%\Logs\FileLogTests-NotFlushedBug-%COMPUTERNAME%-%USERNAME%-CustomLog.log");
            using (var sw = new StreamWriter(logFile, false, Encoding.ASCII))
            {
                sw.AutoFlush = true; // Bug: The file is not actually flushed to disk by setting this.                
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
            var messageLength = 1024;
            var testMessage = new string('T', messageLength);
            var logFile = Environment.ExpandEnvironmentVariables(@"%public%\Logs\FileLogTests-FlushedWorkaround-%COMPUTERNAME%-%USERNAME%-CustomLog.log");
            using (var sw = new StreamWriter(logFile, false, Encoding.ASCII))
            {                
                while (true)
                {                    
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
    }
}