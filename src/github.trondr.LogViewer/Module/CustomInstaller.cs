using System.Collections;
using System.ComponentModel;

namespace github.trondr.LogViewer.Module
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            //Example: Adding a command to windows explorer contect menu
            //this.Context.LogMessage("Adding github.trondr.LogViewer to File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().Install("github.trondr.LogViewer", "Create Something...", Assembly.GetExecutingAssembly().Location, "CreateSomething /exampleParameter=\"%1\"");
            //this.Context.LogMessage("Finnished adding github.trondr.LogViewer to File Explorer context menu.");
            
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            //Example: Removing previously installed command from windows explorer contect menu
            //this.Context.LogMessage("Removing github.trondr.LogViewer from File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().UnInstall("github.trondr.LogViewer");
            //this.Context.LogMessage("Finished removing github.trondr.LogViewer from File Explorer context menu.");
            
            base.Uninstall(savedState);
        }        
    }
}
