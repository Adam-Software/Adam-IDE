using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace AdamController.Behaviors
{
    public class ActivateBehavior : Behavior<Window>
    {
        private bool isActivated;

        public static readonly DependencyProperty ActivatedProperty = DependencyProperty.Register("Activated",
            typeof(bool),
            typeof(ActivateBehavior),
            new PropertyMetadata(OnActivatedChanged)
          );

        public bool Activated
        {
            get => (bool)GetValue(ActivatedProperty);
            set => SetValue(ActivatedProperty, value);
        }

        static void OnActivatedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ActivateBehavior behavior = (ActivateBehavior)dependencyObject;
            if (!behavior.Activated || behavior.isActivated) return;

            // The Activated property is set to true but the Activated event (tracked by the
            // isActivated field) hasn't been fired. Go ahead and activate the window.
            if (behavior.AssociatedObject.WindowState == WindowState.Minimized)
                behavior.AssociatedObject.WindowState = WindowState.Normal;
            _ = behavior.AssociatedObject.Activate();
        }

        protected override void OnAttached()
        {
            AssociatedObject.Activated += OnActivated;
            AssociatedObject.Deactivated += OnDeactivated;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Activated -= OnActivated;
            AssociatedObject.Deactivated -= OnDeactivated;
        }

        private void OnActivated(object sender, EventArgs eventArgs)
        {
            isActivated = true;
            Activated = true;
        }

        private void OnDeactivated(object sender, EventArgs eventArgs)
        {
            isActivated = false;
            Activated = false;
        }

    }
}
