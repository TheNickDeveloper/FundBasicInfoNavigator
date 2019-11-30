using Caliburn.Micro;
using System.Windows;
using FundBasicInfoNavigator.ViewModels;

namespace FundBasicInfoNavigator
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
