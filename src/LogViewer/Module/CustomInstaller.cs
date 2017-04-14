using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using LogViewer.Library.Module.Common.Install;

namespace LogViewer.Module
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        private string _commandId = "github.com.trondr.LogViewer";

        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {            


            this.Context.LogMessage("Adding github.com.trondr.LogViewer to File Explorer context menu...");
            string exeFile = ExeFile();
            new WindowsExplorerContextMenuInstaller().Install(_commandId, "Open log in LogViewer...", exeFile, "OpenLogs /connectionStrings=\"['file://%1']\"");
            this.Context.LogMessage("Finnished adding github.com.trondr.LogViewer to File Explorer context menu.");

            base.Install(stateSaver);
        }

        private static string ExeFile()
        {
            var baseExeFile = Assembly.GetExecutingAssembly().Location;
            var baseExeFileWithoutExtension = Path.GetFileNameWithoutExtension(baseExeFile);
            var guiExeFile = baseExeFileWithoutExtension + ".Gui.exe";
            string exeFile = baseExeFile;
            if(File.Exists(guiExeFile))
            {
                exeFile = guiExeFile;
            }
            return exeFile;
        }

        public override void Uninstall(IDictionary savedState)
        {
            //Example: Removing previously installed command from windows explorer contect menu
            this.Context.LogMessage("Removing github.com.trondr.LogViewer from File Explorer context menu...");
            new WindowsExplorerContextMenuInstaller().UnInstall(_commandId);
            this.Context.LogMessage("Finished removing github.com.trondr.LogViewer from File Explorer context menu.");
            base.Uninstall(savedState);
        }     
    }
}
