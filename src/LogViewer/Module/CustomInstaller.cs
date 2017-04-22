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
            var exeFile = ExeFile();
            new WindowsExplorerContextMenuInstaller().Install(_commandId, "Open log in LogViewer...", exeFile, "OpenLogs /connectionStrings=\"['file://%1']\"");
            Context.LogMessage("Finnished adding github.com.trondr.LogViewer to File Explorer context menu.");

            base.Install(stateSaver);
        }

        private string ExeFile()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var baseExeFile = executingAssembly.Location;
            Context.LogMessage($"Base Exe file: {baseExeFile}"); 
            var baseExeFileWithoutExtension = Path.GetFileNameWithoutExtension(baseExeFile);
            var baseExeFileDirectory = Path.GetDirectoryName(baseExeFile);
            if (string.IsNullOrWhiteSpace(baseExeFileDirectory)) baseExeFileDirectory = string.Empty;
            Context.LogMessage($"Exe directory: {baseExeFileDirectory}");
            var exeFile = baseExeFile;
            var guiExeFileBaseName = baseExeFileWithoutExtension + ".Gui.exe";
            var guiExeFile = Path.Combine(baseExeFileDirectory, guiExeFileBaseName);
            if (File.Exists(guiExeFile))
            {
                exeFile = guiExeFile;
            }
            Context.LogMessage($"Exe file: {exeFile}");
            return exeFile;
        }

        public override void Uninstall(IDictionary savedState)
        {
            //Example: Removing previously installed command from windows explorer contect menu
            Context.LogMessage("Removing github.com.trondr.LogViewer from File Explorer context menu...");
            new WindowsExplorerContextMenuInstaller().UnInstall(_commandId);
            Context.LogMessage("Finished removing github.com.trondr.LogViewer from File Explorer context menu.");
            base.Uninstall(savedState);
        }     
    }
}
