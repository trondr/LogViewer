using System;
using System.Security.Principal;
using Microsoft.Win32;

namespace LogViewer.Library.Module.Common.Install
{
    public class WindowsExplorerContextMenuInstaller : IWindowsExplorerContextMenuInstaller
    {
        public void Install(string commandId, string commandName, string command, string arguments)
        {
            var rootKey = GetRootKey();
            var commandLine = $"\"{command}\" {arguments}";
            var commandIdKeyPath = @"Software\Classes\Folder\shell\" + commandId;
            using (var commandIdKey = rootKey.CreateSubKey(commandIdKeyPath))
            {
                if (commandIdKey == null) throw new NullReferenceException("Failed to create or open registry key: " + commandIdKeyPath);
                commandIdKey.SetValue(null, commandName);
            }

            var commandKeyPath = string.Format(@"Software\Classes\Folder\shell\{0}\command", commandId);
            using (var commandKey = rootKey.CreateSubKey(commandKeyPath))
            {
                if (commandKey == null) throw new NullReferenceException("Failed to create or open registry key: " + commandKeyPath);                
                commandKey.SetValue(null, commandLine, RegistryValueKind.String);
            }

            var commandIdKeyPath2 = @"Software\Classes\*\shell\" + commandId;
            using (var commandIdKey2 = rootKey.CreateSubKey(commandIdKeyPath2))
            {
                if (commandIdKey2 == null) throw new NullReferenceException("Failed to create or open registry key: " + commandIdKeyPath2);
                commandIdKey2.SetValue(null, commandName);
            }

            var commandKeyPath2 = $@"Software\Classes\*\shell\{commandId}\command";
            using (var commandKey2 = rootKey.CreateSubKey(commandKeyPath2))
            {
                if (commandKey2 == null) throw new NullReferenceException("Failed to create or open registry key: " + commandKeyPath2);                
                commandKey2.SetValue(null, commandLine, RegistryValueKind.String);
            }
        }

        public void UnInstall(string commandId)
        {
            if (string.IsNullOrWhiteSpace(commandId)) throw new ArgumentNullException(commandId);
            var commandIdKeyPath = @"Software\Classes\Folder\shell\" + commandId;
            var commandIdKeyPath2 = @"Software\Classes\*\shell\" + commandId;
            var rootKey = GetRootKey();
            rootKey.DeleteSubKeyTree(commandIdKeyPath);
            rootKey.DeleteSubKeyTree(commandIdKeyPath2);            
        }

        private RegistryKey GetRootKey()
        {
            if (IsElevated())
                return Registry.LocalMachine;
            return Registry.CurrentUser;
        }

        private bool IsElevated()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            var isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isElevated;
        }

    }
}