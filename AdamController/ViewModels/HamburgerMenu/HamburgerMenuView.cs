
using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;

namespace AdamController.ViewModels.HamburgerMenu
{

    public class HamburgerMenuView : MainWindowViewModels
    {
        private ObservableCollection<HamburgerMenuItemView> mMenuItems;
        private ObservableCollection<HamburgerMenuItemView> mMenuOptionItems;

        public HamburgerMenuView()
        {
            CreateMenuItems();
        }


        public void CreateMenuItems()
        {
            MenuItems = new ObservableCollection<HamburgerMenuItemView>
            {
                new ScratchControlView(this)
                {
                    Icon = new PackIconSimpleIcons() {Kind = PackIconSimpleIconsKind.Scratch },
                    Label = "Cкретч",
                    ToolTip = "Скретч редактор"
                },
                new ScriptEditorControlView(this)
                {
                    Icon = new PackIconModern() {Kind = PackIconModernKind.PageEdit},
                    Label = "Редактор",
                    ToolTip = "Редактор скриптов"
                },
                new ComputerVisionControlView(this)
                {
                    Icon = new PackIconModern() { Kind = PackIconModernKind.Video },
                    Label = "Компьютерное зрение",
                    ToolTip = "Компьютерное зрение"
                }
            };

            MenuOptionItems = new ObservableCollection<HamburgerMenuItemView>
            {
                new VisualSettingsControlView(this)
                {
                    Icon = new PackIconMaterial() {Kind = PackIconMaterialKind.Cog},
                    Label = "Настройки",
                    ToolTip = "Графические настройки приложения"
                }
            };
        }

        public ObservableCollection<HamburgerMenuItemView> MenuItems
        {
            get => mMenuItems;
            set => SetProperty(ref mMenuItems, value);
        }

        public ObservableCollection<HamburgerMenuItemView> MenuOptionItems
        {
            get => mMenuOptionItems;
            set => SetProperty(ref mMenuOptionItems, value);
        }

    }
}
