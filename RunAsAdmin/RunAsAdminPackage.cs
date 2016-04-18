using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using System.ComponentModel;
using System.IO;

namespace FundaRealEstateBV.RunAsAdmin
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [Guid(GuidList.guidRunAsAdminPkgString)]
    [ProvideOptionPage(typeof(OptionPageGrid), "RunAsAdmin", "General", 0, 0, true)]
    public sealed class RunAsAdminPackage : Package
    {
        private DTE _dte;
        private string _devenvFilename;
        private OptionPageGrid _options;
        private const string RegistryFolder = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\";

        protected override void Initialize()
        {
            base.Initialize();
            _options = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
            _dte = (DTE)GetService(typeof(DTE));
            _devenvFilename = _dte.Application.FileName;

            if (_options.Enabled == true)
            {
                var RunAsAdminKeyLauncher = Registry.GetValue(RegistryFolder, @"C:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\VSLauncher.exe", null);
                if (RunAsAdminKeyLauncher == null)
                {
                    Registry.SetValue(RegistryFolder, @"C:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\VSLauncher.exe", "RUNASADMIN");
                }

                var RunAsAdminKeyDevEnv = Registry.GetValue(RegistryFolder, _devenvFilename, null);
                if (RunAsAdminKeyDevEnv == null || ((string)RunAsAdminKeyDevEnv).Equals("RUNASNORMAL"))
                {
                    Registry.SetValue(RegistryFolder, _devenvFilename, "RUNASADMIN");
                }
            }
            else
            {
                Registry.SetValue(RegistryFolder, _devenvFilename, "RUNASNORMAL");
            }
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class OptionPageGrid : DialogPage
    {
        [Category("RunAsAdmin")]
        [DisplayName(@"Enabled")]
        [Description("Run Visual Studio as administrator")]
        public bool Enabled { get; set; }
    }
}
