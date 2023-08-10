using AdamController.ViewModels.Common;
using MahApps.Metro.Controls;

namespace AdamController.ViewModels.HamburgerMenu
{
    public class HamburgerMenuItemView : BindableBase, IHamburgerMenuItemBase
    {
        private object _icon;
        private object _label;
        private object _toolTip;
        private bool _isVisible = true;

        public HamburgerMenuItemView(HamburgerMenuView hamburgerMenuView)
        {
            HamburgerMenuView = hamburgerMenuView;
        }

        public HamburgerMenuView HamburgerMenuView { get; }

        public object Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public object Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public object ToolTip
        {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
    }
}
