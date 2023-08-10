using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;


namespace AdamController.Behaviors
{
    public class PutCursorAtEndTextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            AssociatedObject.ScrollToEnd();
        }
    }
}
