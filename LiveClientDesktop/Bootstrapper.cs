using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Windows;

namespace LiveClientDesktop
{
    public partial class Bootstrapper : UnityBootstrapper
    {
        private readonly string _startParams;
        public Bootstrapper(string startParams) : base()
        {
            _startParams = startParams;
        }
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Login>();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            configuration.Configure(Container, "defaultContainer");
        }

    }
}
