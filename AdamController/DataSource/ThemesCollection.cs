﻿using AdamBlocklyLibrary.Enum;
using AdamController.Model;
using System.Collections.ObjectModel;

namespace AdamController.DataSource
{
    public class ThemesCollection
    {
        public static ObservableCollection<BlocklyThemeModel> BlocklyThemes { get; private set; } = new()
        {
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Classic, BlocklyThemeName = "Светлая" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Dark, BlocklyThemeName = "Темная" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Highcontrast, BlocklyThemeName = "Повышенный контраст" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Modern, BlocklyThemeName = "Модерн" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Deuteranopia, BlocklyThemeName = "Deuteranopia" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Tritanopia, BlocklyThemeName = "Tritanopia" },
            new BlocklyThemeModel() { BlocklyTheme = BlocklyTheme.Zelos, BlocklyThemeName = "Zelos" },
        };
    }
}
