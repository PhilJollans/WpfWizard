using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using WpfWizard.Classes;

namespace WizardDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var unityContainer = new UnityContainer();
            WizardSettings.Instance.ViewResolver = type => unityContainer.Resolve(type);
        }
    }
}
