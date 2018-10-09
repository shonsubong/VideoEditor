using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Models;
using UWPDemo.Views;

namespace UWPDemo.ViewModels
{
    public class ViewModelLocator
    {
        public const string MainPageKey = "MainPage";

        public ViewModelLocator()
        {
            var nav = new NavigationService();

            nav.Configure(MainPageKey, typeof(MainPage));
                   
            SimpleIoc.Default.Register<INavigationService>(() => nav);           
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IDataService, DataService>();

            SimpleIoc.Default.Register<MainPageViewModel>();
            Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
        }

        public static MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>(Guid.NewGuid().ToString());

        private void NotifyUserMethod(NotificationMessage message)
        {
            //MessageBox.Show(message.Notification);
        }
    }
}
