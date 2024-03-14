﻿using AdamController.Core.Mvvm;
using AdamController.ViewModels.HamburgerMenu;
using MahApps.Metro.IconPacks;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;

namespace AdamController.Modules.HamburgerMenuRegion.ViewModels
{

    public class HamburgerMenuViewModel : RegionViewModelBase
    {
        private ObservableCollection<HamburgerMenuItemView> mMenuItems;
        private ObservableCollection<HamburgerMenuItemView> mMenuOptionItems;

        public HamburgerMenuViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            CreateMenuItems();
        }

        public void CreateMenuItems()
        {
            MenuItems = new ObservableCollection<HamburgerMenuItemView>
            {
                //new ScratchControlView(this)
                //new ScratchControlViewModel()
                //{
                //    Icon = new PackIconSimpleIcons() {Kind = PackIconSimpleIconsKind.Scratch },
                //    Label = "Cкретч",
                //    ToolTip = "Скретч редактор"
                //},
                //new ScriptEditorControlView(this)
                //new ScriptEditorControlViewModel()
                //{
                //    Icon = new PackIconModern() {Kind = PackIconModernKind.PageEdit},
                //    Label = "Редактор",
                //    ToolTip = "Редактор скриптов"
                //},
                //new ComputerVisionControlView(this)
                //new ComputerVisionControlViewModel()
                //{
                //    Icon = new PackIconModern() { Kind = PackIconModernKind.Video },
                 //   Label = "Компьютерное зрение",
                //    ToolTip = "Компьютерное зрение"
                //}
            };

            MenuOptionItems = new ObservableCollection<HamburgerMenuItemView>
            {
                //new VisualSettingsControlView(this)
                //new VisualSettingsControlView()
                //{
                //    Icon = new PackIconMaterial() {Kind = PackIconMaterialKind.Cog},
                //    Label = "Настройки",
                 //   ToolTip = "Графические настройки приложения"
                //}
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