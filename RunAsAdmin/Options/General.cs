using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;

namespace SamirBoulema.RunAsAdmin
{
    internal partial class OptionsProvider
    {
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        private const string RegistryFolder =
            @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\";
        private const string VSLauncherInstallationPath = @"C:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\VSLauncher.exe";
        private readonly string _installationPath = GetVisualStudioInstallationPath();

        [Category("RunAsAdmin")]
        [DisplayName(@"Enabled")]
        [Description("Run Visual Studio as administrator. Restart Visual Studio for the applied changes to take effect")]
        public bool Enabled
        {
            get => IsAdminEnabled();
            set => RunAsAdmin(value);
        }

        private void RunAsAdmin(bool enable)
        {
            Registry.SetValue(RegistryFolder, _installationPath, enable ? "RUNASADMIN" : "RUNASNORMAL");
            Registry.SetValue(RegistryFolder, VSLauncherInstallationPath, enable ? "RUNASADMIN" : "RUNASNORMAL");
        }

        private bool IsAdminEnabled()
        {
            var runAsAdminKeyDevEnv = Registry.GetValue(RegistryFolder, _installationPath, null) as string;

            return runAsAdminKeyDevEnv?.Equals("RUNASADMIN") == true;
        }

        private static string GetVisualStudioInstallationPath()
        {
            var config = (ISetupConfiguration2)
                Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("177F0C4A-1CD3-4DE7-A32C-71DBBB9FA36D")));
            var instance = config.GetInstanceForCurrentProcess();

            var installationFolder = instance.GetInstallationPath();

            return Path.Combine(installationFolder, @"Common7\IDE\devenv.exe");
        }
    }
}
