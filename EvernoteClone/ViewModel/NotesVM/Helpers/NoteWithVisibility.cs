using EvernoteClone.Model;
using System.Windows;

namespace EvernoteClone.ViewModel.Helpers
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class NoteWithVisibility
    {
        public Note Note { get; set; }
        public Visibility Visibility { get; set; } = Visibility.Collapsed;
    }
}