using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Windows;
using System.Web;

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
            InitializeStartupParameters();
            return this.Container.Resolve<Login>();
        }
        private void InitializeStartupParameters()
        {
            if (!string.IsNullOrEmpty(_startParams))
            {
                var startupParams = this.Container.Resolve<StartupParameters>();
                var argsArr = HttpUtility.UrlDecode(_startParams).Replace("powercreator://", "").Split('|');
                startupParams.LiveId = argsArr[0];
                startupParams.Guid = argsArr[1];
                startupParams.Domain = argsArr[2];
            }
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
