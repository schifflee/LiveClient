using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Tests
{
    public abstract class TestBaseWithLocalIocManager : IDisposable
    {
        protected IUnityContainer LocalIocContainer;
        public TestBaseWithLocalIocManager()
        {
            LocalIocContainer = new UnityContainer();
            UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            configuration.Configure(LocalIocContainer, "defaultContainer");
        }
        protected T Resolve<T>()
        {
            return LocalIocContainer.Resolve<T>();
        }
        public virtual void Dispose()
        {
            LocalIocContainer.Dispose();
        }
    }
}
