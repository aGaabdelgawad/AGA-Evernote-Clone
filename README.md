# AGA-Evernote-Clone
WPF Application as a cloning to Evernote application for creating notebooks and notes.

### Code Features
* MVVM pattern.
* No code-behind.
* Code Readability
* Responsive UI.
* Explicit styles in Xaml.

### Note for usage
#### For security purposes, you have to create your own accounts in both Microsoft Azure and Google Firebase:
* Copy your Azure Connection String, Container Name and the Storage Blob Link and paste them in Database/AzureStorageHelper.cs.
* Copy your own Azure Speech Recognition Region and API Key and paste them in Speech() method in ViewModel/NotesVM/NotesVM.cs.
* Copy your Firebase database link and paste it in Database/DatabaseHelper.cs.
* Copy your Firebase Authentication API Key and paste it in Database/FirebaseAuthHelper.cs.
