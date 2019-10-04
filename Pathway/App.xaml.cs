using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using YidanEHRApplication;
using YidanEHRApplication.Views.Login;
using YidanEHRApplication.Views;
using Telerik.Windows.Controls;
using System.Windows;

namespace YidanEHRApplication
{
    public partial class App : Application
    {
        public App()
        {
            StyleManager.ApplicationTheme = new Windows7Theme(); 
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
            //WebContext webContext = new WebContext();
            StyleManager.ApplicationTheme = new Telerik.Windows.Controls.MetroTheme();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                //// This will enable you to bind controls in XAML files to WebContext.Current
                //// properties
                //this.Resources.Add("WebContext", WebContext.Current);
                //// This will automatically authenticate a user when using windows authentication
                //// or when the user chose "Keep me signed in" on a previous login attempt
                //WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);
                //// Show some UI to the user while LoadUser is in progress
                //this.InitializeRootVisual();
                // 将服务地址添加到ResourceDictionary
                foreach (var item in e.InitParams)
                {
                    this.Resources.Add(item.Key, item.Value);
                }

                this.RootVisual = new LogInPage();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
                errorWin.Show();
            }
        }
    }
}