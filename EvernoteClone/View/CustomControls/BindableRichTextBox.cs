using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EvernoteClone.View.CustomControls
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class BindableRichTextBox : RichTextBox
    {
        #region Properties
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));

        public new FlowDocument Document
        {
            get
            {
                return (FlowDocument)GetValue(DocumentProperty);
            }

            set
            {
                SetValue(DocumentProperty, value);
            }
        }
        #endregion

        #region Methods
        public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RichTextBox rtb = (RichTextBox)obj;
            rtb.Document = (FlowDocument)args.NewValue;
        }
        #endregion
    }
}
