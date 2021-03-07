using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.Database;
using System;
using System.ComponentModel;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class LoggingVM : INotifyPropertyChanged
    {
        #region Fields
        private User user;

        private bool regVisibilityFlag = false;

        private Visibility regVis;

        private Visibility logVis;
        #endregion

        #region Properties
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public LoginCommand LoginCommand { get; set; }

        public RegisterCommand RegisterCommand { get; set; }

        public SwitchViewCommand SwitchViewCommand { get; set; }

        public Visibility RegVis
        {
            get { return regVis; }
            set
            {
                regVis = value;
                OnPropertyChanged(nameof(RegVis));
            }
        }

        public Visibility LogVis
        {
            get { return logVis; }
            set
            {
                logVis = value;
                OnPropertyChanged(nameof(LogVis));
            }
        }
        #endregion

        #region Constructors
        public LoggingVM()
        {
            User = new User();

            LoginCommand = new LoginCommand(this);

            RegisterCommand = new RegisterCommand(this);

            SwitchViewCommand = new SwitchViewCommand(this);

            RegVis = Visibility.Collapsed;

            LogVis = Visibility.Visible;
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler LoginCompleted;

        public event EventHandler RegisterCompleted;
        #endregion

        #region Methods
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SwitchViews()
        {
            User = new User();

            regVisibilityFlag = !regVisibilityFlag;

            if (regVisibilityFlag)
            {
                RegVis = Visibility.Visible;
                LogVis = Visibility.Collapsed;
            }
            else
            {
                RegVis = Visibility.Collapsed;
                LogVis = Visibility.Visible;
            }
        }

        public async void Login()
        {
            if (await FirebaseAuthHelper.Login(User))
            {
                MessageBox.Show($"Welcome back, {User.Name}.", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                RaiseLoginCompletedEvent();
            }
        }

        private void RaiseLoginCompletedEvent()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }

        public async void Register()
        {
            if (await FirebaseAuthHelper.Register(User))
            {
                MessageBox.Show($"Welcome joining Evernote, {User.Name}.", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                RaiseRegisterCompletedEvent();
            }
        }

        private void RaiseRegisterCompletedEvent()
        {
            RegisterCompleted?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
