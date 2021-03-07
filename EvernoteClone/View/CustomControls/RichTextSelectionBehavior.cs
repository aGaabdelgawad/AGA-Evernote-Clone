using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Documents;

namespace EvernoteClone.View.CustomControls
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public class RichTextSelectionBehavior : Behavior<BindableRichTextBox>
    {
        #region Properties
        public static readonly DependencyProperty SelectedTextProperty =
           DependencyProperty.Register(
               "SelectedText",
               typeof(TextSelection),
               typeof(RichTextSelectionBehavior),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTextChanged));

        public TextSelection SelectedText
        {
            get { return (TextSelection)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }
        #endregion

        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += RichTextBoxSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= RichTextBoxSelectionChanged;
        }

        void RichTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            SelectedText = AssociatedObject.Selection;
        }

        private static void OnSelectedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is RichTextSelectionBehavior))
                return;
        }
        #endregion
    }
}
