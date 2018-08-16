using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.Logic.Contracts;
using Client.Logic.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    //IUnityContainer container = new UnityContainer();
        //    //container.RegisterType<IChatService, ChatService>();
        //    //container.RegisterType<ILoginService, LoginService>();

        //    //MainWindow mainWindow = container.Resolve<MainWindow>();
        //    LoginView login = new LoginView(new LoginService(), new ChatService());
        //    login.Show();
        //}
    }
}
