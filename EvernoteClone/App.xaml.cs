using EvernoteClone.Model;
using EvernoteClone.View;
using EvernoteClone.ViewModel;
using System.Windows;

namespace EvernoteClone
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loggingWindow = new LoggingWindow();
            var loggingVM = new LoggingVM();

            loggingVM.LoginCompleted += (sender, args) =>
            {
                NotesVM notesVM = new NotesVM();
                notesVM.User = loggingVM.User;

                NotesWindow notesWindow = new NotesWindow();

                notesVM.LogoutCompleted += (sender, args) =>
                {
                    loggingWindow = new LoggingWindow();
                    loggingVM.User = new User();
                    loggingWindow.DataContext = loggingVM;

                    notesWindow.Close();
                    loggingWindow.ShowDialog();
                };

                notesWindow.DataContext = notesVM;

                loggingWindow.Close();
                notesWindow.Show();
            };

            loggingVM.RegisterCompleted += (sender, args) =>
                {
                    NotesVM notesVM = new NotesVM();
                    notesVM.User = loggingVM.User;

                    NotesWindow notesWindow = new NotesWindow();

                    notesVM.LogoutCompleted += (sender, args) =>
                    {
                        loggingWindow = new LoggingWindow();
                        loggingVM.User = new User();
                        loggingVM.SwitchViews();
                        loggingWindow.DataContext = loggingVM;

                        notesWindow.Close();
                        loggingWindow.ShowDialog();
                    };

                    notesWindow.DataContext = notesVM;

                    loggingWindow.Close();
                    notesWindow.Show();
                };

            loggingWindow.DataContext = loggingVM;
            loggingWindow.ShowDialog();
        }
    }
}
