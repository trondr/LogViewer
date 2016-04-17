using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Services;
using github.trondr.LogViewer.Library.Module.Services.TcpLog;
using NUnit.Framework;
using github.trondr.LogViewer.Library.Module.Services.UdpLog;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class UdpLogItemLogItemConnectionStringParserTests
    {


        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void CanParse_UdpConnectionStringWithoutMulticastAddressTest_True()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4";
                const bool expected = true;
                var actual = target.CanParse(connectionString);
                Assert.AreEqual(expected, actual, "Not a valid connection string: " + connectionString);
            }
        }

        [Test]
        public void Parse_UdpConnectionStringWithoutMulticastAddressTest()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4";
                var actual = target.Parse(connectionString);
                Assert.IsInstanceOf<IUdpLogItemConnection>(actual);
                var actual2 = (IUdpLogItemConnection)actual;
                Assert.AreEqual(8099, actual2.Port, "Not a valid udp port");
                Assert.AreEqual(IpVersion.Ipv4, actual2.IpVersion, "Not a valid ip version");
                Assert.AreEqual(null, actual2.MultiCastAddress, "Multi cast address is not null");
                Assert.AreEqual("somehost", actual2.HostName, "Not a valid udp port");
            }
        }

        [Test]
        public void CanParse_UdpConnectionStringWithCorrectMulticastAddressLowestLimit_True()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:224.0.0.0";
                const bool expected = true;
                var actual = target.CanParse(connectionString);
                Assert.AreEqual(expected, actual, "Not a valid connection string: " + connectionString);
            }
        }

        [Test]
        public void Parse_UdpConnectionStringWithCorrectMulticastAddressLowestLimit()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:224.0.0.0";
                var actual = target.Parse(connectionString);
                Assert.IsInstanceOf<IUdpLogItemConnection>(actual);
                var actual2 = (IUdpLogItemConnection)actual;
                Assert.AreEqual(8099, actual2.Port, "Not a valid udp port");
                Assert.AreEqual(IpVersion.Ipv4, actual2.IpVersion, "Not a valid ip version");
                Assert.AreEqual("somehost", actual2.HostName, "Not a valid udp port");
                Assert.IsNotNull(actual2.MultiCastAddress, "Multi cast address is null");
                Assert.AreEqual("224.0.0.0", actual2.MultiCastAddress.ToString(), "Not expected multi cast address");
            }
        }

        [Test]
        public void CanParse_UdpConnectionStringWithInCorrectMulticastAddressLowestLimitMinusOne_False()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:223.255.255.255";
                const bool expected = false;
                var actual = target.CanParse(connectionString);
                Assert.AreEqual(expected, actual, "Connection string is valid: " + connectionString);
            }
        }

        [Test]
        public void Parse_UdpConnectionStringWithInCorrectMulticastAddressLowestLimitMinusOne()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:223.255.255.255";
                Assert.Throws<InvalidConnectionStringException>(() => { var actual = target.Parse(connectionString);});                
            }
        }
        
        [Test]
        public void CanParse_UdpConnectionStringWithCorrectMulticastAddressHighesLimit_True()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:239.255.255.255";
                const bool expected = true;
                var actual = target.CanParse(connectionString);
                Assert.AreEqual(expected, actual, "Not a valid connection string: " + connectionString);
            }
        }

        [Test]
        public void Parse_UdpConnectionStringWithCorrectMulticastAddressHighesLimit()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:239.255.255.255";
                var actual = target.Parse(connectionString);
                Assert.IsInstanceOf<IUdpLogItemConnection>(actual);
                var actual2 = (IUdpLogItemConnection)actual;
                Assert.AreEqual(8099, actual2.Port, "Not a valid udp port");
                Assert.AreEqual(IpVersion.Ipv4, actual2.IpVersion, "Not a valid ip version");
                Assert.AreEqual("somehost", actual2.HostName, "Not a valid udp port");
                Assert.IsNotNull(actual2.MultiCastAddress, "Multi cast address is null");
                Assert.AreEqual("239.255.255.255", actual2.MultiCastAddress.ToString(), "Not expected multi cast address");
            }
        }
        

        [Test]
        public void CanParse_UdpConnectionStringWithInCorrectMulticastAddressHighesLimitPlusOne_False()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:240.0.0.0";
                const bool expected = false;
                var actual = target.CanParse(connectionString);
                Assert.AreEqual(expected, actual, "Connection string is valid: " + connectionString);
            }
        }

        [Test]
        public void Parse_UdpConnectionStringWithInCorrectMulticastAddressHighesLimitPlusOne()
        {
            using (var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<IUdpLogItemConnectionStringParser>();
                const string connectionString = "udp:somehost:8099:ipv4:240.0.0.0";
                Assert.Throws<InvalidConnectionStringException>(() => { var actual = target.Parse(connectionString);});                
            }
        }
    }
}