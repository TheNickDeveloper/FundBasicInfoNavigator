using Caliburn.Micro;
using System.Windows;
using WpfDataGrid.ViewModels;

namespace WpfDataGrid
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sneder, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
