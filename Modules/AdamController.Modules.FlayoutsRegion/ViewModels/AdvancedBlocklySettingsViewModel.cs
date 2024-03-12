﻿using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.FlayoutsRegion.ViewModels.Flayouts
{
    public class AdvancedBlocklySettingsViewModel : RegionViewModelBase
    {
        public AdvancedBlocklySettingsViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }
    }
}
