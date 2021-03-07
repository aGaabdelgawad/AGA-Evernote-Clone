using EvernoteClone.ViewModel.Helpers;
using System;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class EndEditNoteCommand : ICommand
    {
        #region Properties
        public NotesVM VM { get; set; }
        #endregion

        #region Constructors
        public EndEditNoteCommand(NotesVM vm)
        {
            VM = vm;
        }
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region Methods
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is NoteWithVisibility noteWithVisibility)
            {
                VM.EndNoteEditing(noteWithVisibility.Note, noteWithVisibility.Note.NotebookId);
            }
        }
        #endregion
    }
}
