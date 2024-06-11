using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace AdamStudio.Core.Behaviors
{
    public class AvalonEditBehaviour : Behavior<TextEditor>
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AvalonEditBehaviour),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject == null) return;
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject == null) return;
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            TextEditor textEditor = sender as TextEditor;
            if (textEditor != null)
            {
                if (textEditor.Document == null) return;
                Text = textEditor.Document.Text;
            }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            AvalonEditBehaviour behavior = dependencyObject as AvalonEditBehaviour;
            if (behavior.AssociatedObject != null)
            {
                TextEditor editor = behavior.AssociatedObject;
                if (editor.Document != null)
                {
                    try
                    {
                        int caretOffset = editor.CaretOffset;
                        editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
                        editor.CaretOffset = caretOffset;
                    }
                    catch { }
                    
                }
            }
        }
    }
}
