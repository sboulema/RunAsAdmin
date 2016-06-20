using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace SamirBoulema.RunAsAdmin
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
        private const string RegistryFolder = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\";

        protected override void Initialize()
        {
            base.Initialize();
            var options = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
            options.SetPackage(this);
            _dte = (DTE)GetService(typeof(DTE));
            _devenvFilename = _dte.Application.FileName;

            var regEntry = Registry.GetValue(RegistryFolder, _devenvFilename, null);

            if ((options.Enabled == false && regEntry == null) ||
                (options.Enabled && (regEntry == null || !regEntry.Equals("RUNASADMIN"))))
            {
                options.Enabled = true;
                options.SaveSettingsToStorage();
                RestartVisualStudio();
            }
        }

        public void RunAsAdmin(bool enable)
        {
            if (enable)
            {
                var runAsAdminKeyDevEnv = Registry.GetValue(RegistryFolder, _devenvFilename, null);
                if (runAsAdminKeyDevEnv == null || ((string)runAsAdminKeyDevEnv).Equals("RUNASNORMAL"))
                {
                    Registry.SetValue(RegistryFolder, _devenvFilename, "RUNASADMIN");
                }
            }
            else
            {
                Registry.SetValue(RegistryFolder, _devenvFilename, "RUNASNORMAL");
            }
        }

        public void RestartVisualStudio()
        {
            // Start new instance of Visual Studio
            try
            {
                System.Diagnostics.Process.Start(_dte.Application.FileName, _dte.Application.CommandLineArguments);
            }
            catch (Exception)
            {
                // User cancelled starting the new instance
            }          

            // Close old instance
            _dte.ExecuteCommand("File.Exit");
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public sealed class OptionPageGrid : DialogPage
    {
        private RunAsAdminPackage _package;

        public void SetPackage(RunAsAdminPackage package)
        {
            _package = package;
        }

        private bool _enabled;

        [Category("RunAsAdmin")]
        [DisplayName(@"Enabled")]
        [Description("Run Visual Studio as administrator. Restart Visual Studio for the applied changes to take effect")]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                _package?.RunAsAdmin(value);
            }
        }
    }
}
