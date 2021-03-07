using EvernoteClone.ViewModel.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace EvernoteClone.View.UserControls
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    /// <summary>
    /// Interaction logic for DisplayNotebookControl.xaml
    /// </summary>
    public partial class DisplayNotebookControl : UserControl
    {
        #region Properties
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookWithVisProperty =
            DependencyProperty.Register("NotebookWithVis", typeof(NotebookWithVisibility), typeof(DisplayNotebookControl), new PropertyMetadata(null, (d, e) =>
            {
                if (d is DisplayNotebookControl displayNotebookControl)
                {
                    displayNotebookControl.DataContext = displayNotebookControl.NotebookWithVis;
                }
            }
            ));

        public NotebookWithVisibility NotebookWithVis
        {
            get { return (NotebookWithVisibility)GetValue(NotebookWithVisProperty); }
            set { SetValue(NotebookWithVisProperty, value); }
        }

        #endregion

        #region Constructors
        public DisplayNotebookControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
