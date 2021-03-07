using Azure.Storage.Blobs;
using EvernoteClone.Model;
using EvernoteClone.View.CustomControls;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using EvernoteClone.Database;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EvernoteClone.ViewModel
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class NotesVM : INotifyPropertyChanged
    {
        #region Fields
        private User user;

        private string addNewNotebookImagePath;

        private string addNewNoteImagePath;

        private ObservableCollection<NotebookWithVisibility> notebooksWithVisibility;

        private NoteWithVisibility selectedNoteWithVis;

        private BindableRichTextBox richTextBox;

        private FlowDocument noteContent;

        private string statusBarText;

        private TextSelection selectedText;

        private bool? boldButtonIsChecked;

        private bool? italicButtonIsChecked;

        private bool? underlineButtonIsChecked;

        private object selectedFontFamily;

        private string selectedFontSize;

        private object selectedFontColor;

        private object selectedBackgroundColor;
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

        public string AddNewNotebookImagePath
        {
            get { return addNewNotebookImagePath; }
            set
            {
                addNewNotebookImagePath = value;
                OnPropertyChanged(nameof(AddNewNotebookImagePath));
            }
        }

        public string AddNewNoteImagePath
        {
            get { return addNewNoteImagePath; }
            set
            {
                addNewNoteImagePath = value;
                OnPropertyChanged(nameof(AddNewNoteImagePath));
            }
        }

        public ObservableCollection<NotebookWithVisibility> NotebooksWithVisibility
        {
            get { return notebooksWithVisibility; }
            set
            {
                notebooksWithVisibility = value;
                OnPropertyChanged(nameof(NotebooksWithVisibility));
            }
        }

        public NoteWithVisibility SelectedNoteWithVis
        {
            get { return selectedNoteWithVis; }
            set
            {
                selectedNoteWithVis = value;
                OnPropertyChanged(nameof(SelectedNoteWithVis));
                ShowNote();
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }

        public BindableRichTextBox RichTextBox
        {
            get { return richTextBox; }
            set
            {
                richTextBox = value;
                OnPropertyChanged(nameof(RichTextBox));
            }
        }

        public FlowDocument NoteContent
        {
            get { return noteContent; }
            set
            {
                noteContent = value;
                OnPropertyChanged(nameof(NoteContent));
            }
        }

        public string StatusBarText
        {
            get { return statusBarText; }
            set
            {
                statusBarText = value;
                OnPropertyChanged(nameof(StatusBarText));
            }
        }

        public TextSelection SelectedText
        {
            get { return selectedText; }
            set
            {
                selectedText = value;
                OnPropertyChanged(nameof(SelectedText));
                if (!String.IsNullOrEmpty(selectedText.Text))
                    UpdateTextFormatOptions();
            }
        }

        public bool? BoldButtonIsChecked
        {
            get { return boldButtonIsChecked; }
            set
            {
                boldButtonIsChecked = value;
                OnPropertyChanged(nameof(BoldButtonIsChecked));
                ChangeBoldForSelectedText();
            }
        }

        public bool? ItalicButtonIsChecked
        {
            get { return italicButtonIsChecked; }
            set
            {
                italicButtonIsChecked = value;
                OnPropertyChanged(nameof(ItalicButtonIsChecked));
                ChangeItalicForSelectedText();
            }
        }

        public bool? UnderlineButtonIsChecked
        {
            get { return underlineButtonIsChecked; }
            set
            {
                underlineButtonIsChecked = value;
                OnPropertyChanged(nameof(UnderlineButtonIsChecked));
                ChangeUnderlineForSelectedText();
            }
        }

        public ICollection<FontFamily> FontFamilies { get; set; }

        public object SelectedFontFamily
        {
            get { return selectedFontFamily; }
            set
            {
                selectedFontFamily = value;
                OnPropertyChanged(nameof(SelectedFontFamily));
                ChangeFontFamilyForSelectedText();
            }
        }

        public List<double> FontSizes { get; set; }

        public string SelectedFontSize
        {
            get { return selectedFontSize; }
            set
            {
                selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
                ChangeFontSizeForSelectedText();
            }
        }

        public IEnumerable<string> FontColors { get; set; }

        public object SelectedFontColor
        {
            get { return selectedFontColor; }
            set
            {
                selectedFontColor = value;
                OnPropertyChanged(nameof(SelectedFontColor));
                ChangeFontColorForSelectedText();
            }
        }

        public object SelectedBackgroundColor
        {
            get { return selectedBackgroundColor; }
            set
            {
                selectedBackgroundColor = value;
                OnPropertyChanged(nameof(SelectedBackgroundColor));
                ChangeBackgroundColorForSelectedText();
            }
        }

        public NewNotebookCommand NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public ExitCommand ExitCommand { get; set; }

        public AboutCommand AboutCommand { get; set; }

        public VisitLinkCommand VisitLinkCommand { get; set; }

        public StartEditNotebookCommand StartEditNotebookCommand { get; set; }

        public EndEditNotebookCommand EndEditNotebookCommand { get; set; }

        public StartEditNoteCommand StartEditNoteCommand { get; set; }

        public EndEditNoteCommand EndEditNoteCommand { get; set; }

        public DeleteNotebookCommand DeleteNotebookCommand { get; set; }

        public DeleteNoteCommand DeleteNoteCommand { get; set; }

        public SaveCommand SaveCommand { get; set; }

        public SaveAsCommand SaveAsCommand { get; set; }

        public UpdateStatusBarCommand UpdateStatusBarCommand { get; set; }

        public FullScreenCommand FullScreenCommand { get; set; }

        public NormalScreenCommand NormalScreenCommand { get; set; }

        public InsertImageCommand InsertImageCommand { get; set; }

        public SpeechCommand SpeechCommand { get; set; }

        public LogoutCommand LogoutCommand { get; set; }
        #endregion

        #region Constructors
        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);

            NewNoteCommand = new NewNoteCommand(this);

            ExitCommand = new ExitCommand(this);

            AboutCommand = new AboutCommand(this);

            VisitLinkCommand = new VisitLinkCommand(this);

            StartEditNotebookCommand = new StartEditNotebookCommand(this);

            EndEditNotebookCommand = new EndEditNotebookCommand(this);

            StartEditNoteCommand = new StartEditNoteCommand(this);

            EndEditNoteCommand = new EndEditNoteCommand(this);

            DeleteNotebookCommand = new DeleteNotebookCommand(this);

            DeleteNoteCommand = new DeleteNoteCommand(this);

            SaveCommand = new SaveCommand(this);

            SaveAsCommand = new SaveAsCommand(this);

            UpdateStatusBarCommand = new UpdateStatusBarCommand(this);

            FullScreenCommand = new FullScreenCommand(this);

            NormalScreenCommand = new NormalScreenCommand(this);

            InsertImageCommand = new InsertImageCommand(this);

            SpeechCommand = new SpeechCommand(this);

            LogoutCommand = new LogoutCommand(this);

            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>();

            NoteContent = new FlowDocument();

            FontFamilies = Fonts.SystemFontFamilies;

            FontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            FontColors = typeof(Brushes)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(x => x.Name);

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            GetNotebooks();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectedNoteChanged;

        public event EventHandler LogoutCompleted;
        #endregion

        #region Methods
        public async void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = String.Format("Notebook {0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")),
                UserId = User.Id
            };

            if (await DatabaseHelper.Insert(newNotebook))
            {
                var notebooks = await DatabaseHelper.ReadToList<Notebook>();

                if (notebooks != null)
                {
                    var reqNotebook = notebooks.FirstOrDefault(nb => nb.Name == newNotebook.Name);

                    NotebooksWithVisibility.Add(new NotebookWithVisibility()
                    {
                        Notebook = reqNotebook,
                        NotesWithVisibility = new List<NoteWithVisibility>()
                    });
                }
            }
        }

        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                Title = String.Format("Note {0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            if (await DatabaseHelper.Insert(newNote))
            {
                var notes = await DatabaseHelper.ReadToList<Note>();

                if (notes != null)
                {
                    var reqNote = notes.FirstOrDefault(n => n.Title == newNote.Title);

                    string noteName = $"{reqNote.Title}_{reqNote.Id}.rtf";
                    string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, noteName);

                    try
                    {
                        using (FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
                        {
                            var flowDoc = new FlowDocument();
                            var content = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
                            content.Save(fileStream, DataFormats.Rtf);
                        }

                        reqNote.FileLocation = await AzureStorageHelper.UpdateBlob(noteName, rtfFile);

                        if (await DatabaseHelper.Update(reqNote))
                        {
                            NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).NotesWithVisibility.Add(new NoteWithVisibility()
                            {
                                Note = reqNote
                            });

                            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Note not saved!", "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public void Exit()
        {
            if (MessageBox.Show("Are you sure you want to close?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        public void About()
        {
            string aboutMessage = $"Evernote Clone 2021{Environment.NewLine}Version 1.0.0{Environment.NewLine}© 2021 Ahmed Gamal Abdel Gawad.{Environment.NewLine}All rights reserved.";
            MessageBox.Show(aboutMessage, "About Evernote Clone", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void VisitLink()
        {
            var uri = "http://www.linkedin.com/in/agaabdelgawad/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }

        public void StartNotebookEditing(Notebook notebook)
        {
            NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebook.Id).Visibility = Visibility.Visible;

            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
        }

        public async void EndNotebookEditing(Notebook notebook)
        {
            try
            {
                if (await DatabaseHelper.Update(notebook))
                {
                    NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebook.Id).Notebook.Name = notebook.Name;
                    NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebook.Id).Visibility = Visibility.Collapsed;

                    NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
                }
            }
            catch
            {
                MessageBox.Show("Notebook not renamed!", "Error Renaming", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void GetNotebooks()
        {
            var notebooks = await DatabaseHelper.ReadToList<Notebook>();

            if (notebooks != null)
            {
                var reqNotebooks = notebooks.Where(nb => nb.UserId == User.Id).ToList();

                if (reqNotebooks != null)
                {
                    NotebooksWithVisibility.Clear();

                    for (int i = 0; i < reqNotebooks.Count; i++)
                    {
                        NotebooksWithVisibility.Add(new NotebookWithVisibility()
                        {
                            Notebook = reqNotebooks[i],
                            NotesWithVisibility = new List<NoteWithVisibility>()
                        });

                        var notes = await DatabaseHelper.ReadToList<Note>();

                        if (notes != null)
                        {
                            var reqNotes = notes.Where(n => n.NotebookId == reqNotebooks[i].Id).ToList();

                            if (reqNotes != null)
                            {
                                for (int j = 0; j < reqNotes.Count; j++)
                                {
                                    NotebooksWithVisibility[i].NotesWithVisibility.Add(new NoteWithVisibility()
                                    {
                                        Note = reqNotes[j]
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        public void StartNoteEditing(Note note, string notebookId)
        {
            NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).NotesWithVisibility.FirstOrDefault(n => n.Note.Id == note.Id).Visibility = Visibility.Visible;

            NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).IsExpanded = true;

            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
        }

        public async void EndNoteEditing(Note note, string notebookId)
        {
            try
            {
                note.UpdatedAt = DateTime.Now;

                if (await DatabaseHelper.Update(note))
                {
                    var reqNote = NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).NotesWithVisibility.FirstOrDefault(n => n.Note.Id == note.Id);

                    reqNote.Note.Title = note.Title;

                    reqNote.Note.UpdatedAt = note.UpdatedAt;

                    reqNote.Visibility = Visibility.Collapsed;

                    NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
                }
            }
            catch
            {
                MessageBox.Show("Note not renamed!", "Error Renaming", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void DeleteNotebook(Notebook notebook)
        {
            var Result = MessageBox.Show("Are you sure you want to permanently delete this notebook?", "Delete Notebook", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (Result == MessageBoxResult.Yes)
            {
                try
                {
                    if (await DatabaseHelper.Delete<Notebook>(notebook))
                    {
                        var reqNotes = NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebook.Id).NotesWithVisibility;

                        foreach (var noteVis in reqNotes)
                        {
                            string noteName = $"{noteVis.Note.Title}_{noteVis.Note.Id}.rtf";

                            if (await AzureStorageHelper.DeleteBlob(noteName))
                            {
                                await DatabaseHelper.Delete<Note>(noteVis.Note);
                            }
                        }

                        if (NotebooksWithVisibility.Remove(NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebook.Id)))
                        {
                            MessageBox.Show("Notebook deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Notebook not deleted!", "Error Deleting", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void DeleteNote(Note note, string notebookId)
        {
            var Result = MessageBox.Show("Are you sure you want to permanently delete this note?", "Delete Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (Result == MessageBoxResult.Yes)
            {
                string noteName = $"{note.Title}_{note.Id}.rtf";
                try
                {
                    await AzureStorageHelper.DeleteBlob(noteName);

                    var reqNote = NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).NotesWithVisibility.FirstOrDefault(n => n.Note.Id == note.Id);

                    if (await DatabaseHelper.Delete<Note>(note))
                    {
                        if (NotebooksWithVisibility.FirstOrDefault(nb => nb.Notebook.Id == notebookId).NotesWithVisibility.Remove(reqNote))
                        {
                            MessageBox.Show("Note deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Note not deleted!", "Error Deleting", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void SaveNote()
        {
            if (SelectedNoteWithVis != null)
            {
                string noteName = $"{SelectedNoteWithVis.Note.Title}_{SelectedNoteWithVis.Note.Id}.rtf";
                string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, noteName);

                try
                {
                    using (FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
                    {
                        var content = new TextRange(NoteContent.ContentStart, NoteContent.ContentEnd);
                        content.Save(fileStream, DataFormats.Rtf);
                    }

                    SelectedNoteWithVis.Note.FileLocation = await AzureStorageHelper.UpdateBlob(noteName, rtfFile);

                    SelectedNoteWithVis.Note.UpdatedAt = DateTime.Now;

                    if (await DatabaseHelper.Update(SelectedNoteWithVis.Note))
                    {
                        NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);

                        MessageBox.Show("Note changes saved succesfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Note not saved!", "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a note first!", "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public async void DownloadNote()
        {
            if (SelectedNoteWithVis != null)
            {
                string noteName = $"{SelectedNoteWithVis.Note.Title}_{SelectedNoteWithVis.Note.Id}.rtf";
                string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, noteName);

                try
                {
                    using (FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
                    {
                        var content = new TextRange(NoteContent.ContentStart, NoteContent.ContentEnd);
                        content.Save(fileStream, DataFormats.Rtf);
                    }

                    SelectedNoteWithVis.Note.FileLocation = await AzureStorageHelper.UpdateBlob(noteName, rtfFile);

                    SelectedNoteWithVis.Note.UpdatedAt = DateTime.Now;

                    if (await DatabaseHelper.Update(SelectedNoteWithVis.Note))
                    {
                        var saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Word Document (*.doc)|*.doc|Rich Text Format (*.rtf)|*.rtf";

                        if (saveDialog.ShowDialog() == true)
                        {
                            AzureStorageHelper.DownloadBlob(noteName, saveDialog.FileName);

                            MessageBox.Show("Note saved succesfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                            NotebooksWithVisibility = new ObservableCollection<NotebookWithVisibility>(NotebooksWithVisibility);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Note not saved!", "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a note first!", "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void ShowNote()
        {
            NoteContent.Blocks.Clear();
            if (SelectedNoteWithVis != null)
            {
                if (!string.IsNullOrEmpty(SelectedNoteWithVis.Note.FileLocation))
                {
                    string downloadPath = $"{SelectedNoteWithVis.Note.Title}_{SelectedNoteWithVis.Note.Id}.rtf";
                    await new BlobClient(new Uri(SelectedNoteWithVis.Note.FileLocation)).DownloadToAsync(downloadPath);
                    using (FileStream fileStream = new FileStream(downloadPath, FileMode.Open))
                    {
                        var content = new TextRange(NoteContent.ContentStart, NoteContent.ContentEnd);
                        content.Load(fileStream, DataFormats.Rtf);
                    }
                }
            }
        }

        public void UpdateStatusBar()
        {
            int textLength = new TextRange(NoteContent.ContentStart, NoteContent.ContentEnd).Text.Length - 2;

            if (textLength < 0)
                textLength = 0;
            StatusBarText = $"{textLength} characters written.";
        }

        private void UpdateTextFormatOptions()
        {
            var selectedTextWeight = SelectedText.GetPropertyValue(Inline.FontWeightProperty);
            if (selectedTextWeight != DependencyProperty.UnsetValue)
                BoldButtonIsChecked = selectedTextWeight != DependencyProperty.UnsetValue && selectedTextWeight.Equals(FontWeights.Bold);

            var selectedTextStyle = SelectedText.GetPropertyValue(Inline.FontStyleProperty);
            if (selectedTextStyle != DependencyProperty.UnsetValue)
                ItalicButtonIsChecked = selectedTextStyle != DependencyProperty.UnsetValue && selectedTextStyle.Equals(FontStyles.Italic);

            var selectedTextDecoration = SelectedText.GetPropertyValue(Inline.TextDecorationsProperty);
            if (selectedTextDecoration != DependencyProperty.UnsetValue)
                UnderlineButtonIsChecked = selectedTextDecoration != DependencyProperty.UnsetValue && selectedTextDecoration.Equals(TextDecorations.Underline);

            var selectedTextFontFamily = SelectedText.GetPropertyValue(Inline.FontFamilyProperty);
            if (selectedTextFontFamily != DependencyProperty.UnsetValue)
                SelectedFontFamily = selectedTextFontFamily;
            else
                SelectedFontFamily = null;

            var selectedTextFontSize = SelectedText.GetPropertyValue(Inline.FontSizeProperty);
            if (selectedTextFontSize != DependencyProperty.UnsetValue)
                SelectedFontSize = (SelectedText.GetPropertyValue(Inline.FontSizeProperty)).ToString();
            else
                SelectedFontSize = string.Empty;

            var selectedTextFontColor = SelectedText.GetPropertyValue(Inline.ForegroundProperty);
            if (selectedTextFontColor != DependencyProperty.UnsetValue)
            {
                var colorName = typeof(Colors)
            .GetProperties()
            .Where(p => (Color)p
            .GetValue(null, null) == (selectedTextFontColor as SolidColorBrush).Color)
            .Select(p => p.Name)
            .FirstOrDefault();

                SelectedFontColor = colorName;
            }
            else
                SelectedFontColor = null;

            var selectedTextBackgroundColor = SelectedText.GetPropertyValue(Inline.BackgroundProperty);
            if (selectedTextBackgroundColor != null && selectedTextBackgroundColor != DependencyProperty.UnsetValue)
            {
                var bgColorName = typeof(Colors)
            .GetProperties()
            .Where(p => (Color)p
            .GetValue(null, null) == (selectedTextBackgroundColor as SolidColorBrush).Color)
            .Select(p => p.Name)
            .FirstOrDefault();

                SelectedBackgroundColor = bgColorName;
            }
            else
                SelectedBackgroundColor = null;
        }

        private void ChangeBoldForSelectedText()
        {
            bool isChecked = BoldButtonIsChecked ?? false;

            if (isChecked)
                SelectedText.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                SelectedText.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void ChangeItalicForSelectedText()
        {
            bool isChecked = ItalicButtonIsChecked ?? false;

            if (isChecked)
                SelectedText.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                SelectedText.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void ChangeUnderlineForSelectedText()
        {
            bool isChecked = UnderlineButtonIsChecked ?? false;

            if (isChecked)
                SelectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                var textDecoColl = (SelectedText.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection);
                if (textDecoColl != null)
                {
                    textDecoColl.TryRemove(TextDecorations.Underline, out TextDecorationCollection textDecorations);
                    if (textDecorations != null)
                        SelectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
                }
            }
        }

        private void ChangeFontFamilyForSelectedText()
        {
            if (SelectedFontFamily != null)
                SelectedText.ApplyPropertyValue(Inline.FontFamilyProperty, SelectedFontFamily);
        }

        private void ChangeFontSizeForSelectedText()
        {
            bool isNumber = Double.TryParse(SelectedFontSize, out double textSize);
            if (isNumber)
                SelectedText.ApplyPropertyValue(Inline.FontSizeProperty, textSize.ToString());
        }

        private void ChangeFontColorForSelectedText()
        {
            if (SelectedFontColor != null)
                SelectedText.ApplyPropertyValue(Inline.ForegroundProperty, SelectedFontColor);
        }

        private void ChangeBackgroundColorForSelectedText()
        {
            if (SelectedBackgroundColor != null)
                SelectedText.ApplyPropertyValue(Inline.BackgroundProperty, SelectedBackgroundColor);
        }

        public void MaximizeWindow(Window window)
        {
            window.WindowState = WindowState.Maximized;
        }

        public void NormalizeWindow(Window window)
        {
            window.WindowState = WindowState.Normal;
        }

        public void InsertImage()
        {
            OpenFileDialog insertDialog = new OpenFileDialog();

            insertDialog.Title = "Insert Picture";
            insertDialog.Filter = "All Pictures|*.jpg;*.jpeg;*.png;*.bmp;*.gif|JPEG Format (*.jpg, *.jpeg)|*.jpg|Portable Network Graphics (*.png)|*.png|Windows Bitmap (*.bmp)|*.bmp|Graphics Interchage Format (*.gif)|*.gif";
            if (insertDialog.ShowDialog() == true)
            {
                Paragraph paragraph = new Paragraph();

                BitmapImage bitmapImage = new BitmapImage(new Uri(insertDialog.FileName));

                Image image = new Image();
                image.Source = bitmapImage;
                image.Width = bitmapImage.Width;
                image.Height = bitmapImage.Height;
                image.Stretch = Stretch.Uniform;

                paragraph.Inlines.Add(image);

                NoteContent.Blocks.Add(paragraph);
            }
        }

        public async void Speech()
        {
            string region = "Your_Region_Key";
            string key = "Your_Azure_Speech_Recogintion_Key";

            var speechConfig = SpeechConfig.FromSubscription(key, region);
            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    NoteContent.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }
        }

        public void Logout()
        {
            RaiseLogoutCompletedEvent();
        }

        private void RaiseLogoutCompletedEvent()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                LogoutCompleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
