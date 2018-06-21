using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;

namespace SamirBoulema.RunAsAdmin
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidRunAsAdminPkgString)]
    [ProvideOptionPage(typeof(OptionPageGrid), "RunAsAdmin", "General", 0, 0, true)]
    public sealed class RunAsAdminPackage : AsyncPackage
    {
        protected override async System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken, 
            IProgress<ServiceProgressData> progress)
        {
            DteHelper.Dte = await GetServiceAsync(typeof(DTE)) as DTE;
            await base.InitializeAsync(cancellationToken, progress);
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public sealed class OptionPageGrid : DialogPage
    {
        private const string RegistryFolder = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\";

        [Category("RunAsAdmin")]
        [DisplayName(@"Enabled")]
        [Description("Run Visual Studio as administrator. Restart Visual Studio for the applied changes to take effect")]
        public bool Enabled
        {
            get => IsAdminEnabled();
            set => RunAsAdmin(value);
        }

        private void RunAsAdmin(bool enable) 
            => Registry.SetValue(RegistryFolder, DteHelper.Dte.Application.FileName, (enable ? "RUNASADMIN" : "RUNASNORMAL"));

        private bool IsAdminEnabled()
        {
            var runAsAdminKeyDevEnv = Registry.GetValue(RegistryFolder, DteHelper.Dte.Application.FileName, null) as string;

            return !string.IsNullOrEmpty(runAsAdminKeyDevEnv) && runAsAdminKeyDevEnv.Equals("RUNASADMIN");
        }
    }
}
