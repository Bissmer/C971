using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

    }
}
