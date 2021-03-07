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
    /// Interaction logic for DisplayNoteControl.xaml
    /// </summary>
    public partial class DisplayNoteControl : UserControl
    {
        #region Properties
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteWithVisProperty =
            DependencyProperty.Register("NoteWithVis", typeof(NoteWithVisibility), typeof(DisplayNoteControl), new PropertyMetadata(null, (d, e) =>
            {
                if (d is DisplayNoteControl displayNoteControl)
                {
                    displayNoteControl.DataContext = displayNoteControl.NoteWithVis;
                }
            }
            ));

        public NoteWithVisibility NoteWithVis
        {
            get { return (NoteWithVisibility)GetValue(NoteWithVisProperty); }
            set { SetValue(NoteWithVisProperty, value); }
        }
        #endregion

        #region Constructors
        public DisplayNoteControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
