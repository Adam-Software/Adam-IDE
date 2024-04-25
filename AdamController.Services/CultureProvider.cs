using AdamController.Services.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AdamController.Services
{
    public class CultureProvider :  ICultureProvider
    {
        #region Const

        private const string cEnString = "en-EN";
        private const string cRuString = "ru-RU";

        #endregion

        #region ~

        public CultureProvider() {}

        #endregion

        #region Public fields

        public List<CultureInfo> AppSupportCultureInfos { get { return GetSupportAppCultures(); } }

        public CultureInfo CurrentAppCultureInfo {  get; private set; }

        #endregion

        #region Public methods

        public void LoadCultureInfoDictonary(CultureInfo culture)
        {
            string resourceName = string.Empty;
            List<CultureInfo> supportedCulture = AppSupportCultureInfos;

            foreach (CultureInfo cultureInfo in supportedCulture)
            {
                if (cultureInfo.Name == culture?.Name)
                {
                    resourceName = $"pack://application:,,,/AdamController.Core;component/LocalizationDictionary/{cultureInfo.TwoLetterISOLanguageName}.xaml";
                }
            }

            ResourceDictionary current = FindLoadedDictonary();

            if(current.Count != 0) 
            {
                Application.Current.Resources.MergedDictionaries.Remove(current);
            }

            Uri uri = new(resourceName);

            var resources = new ResourceDictionary
            {
                Source = uri
            };

            Application.Current.Resources.MergedDictionaries.Add(resources);

            SetCurrentCulture(culture);
        }

        #endregion

        private void SetCurrentCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CurrentAppCultureInfo = culture;
        }


        public void Dispose()
        {
            
        }

        #region Private method

        private ResourceDictionary FindLoadedDictonary()
        {
            var supportedCulture = AppSupportCultureInfos;

            foreach (var cultureInfo in supportedCulture)
            {
                string resourceName = $"pack://application:,,,/AdamController.Core;component/LocalizationDictionary/{cultureInfo.TwoLetterISOLanguageName}.xaml";
                var uri = new Uri(resourceName);
                ResourceDictionary current = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == uri);

                if (current != null)
                    return current;
            }

            return new ResourceDictionary();
        }

        private static List<CultureInfo> GetSupportAppCultures()
        {
            CultureInfo en = new(cEnString);
            CultureInfo ru = new(cRuString);

            List<CultureInfo> cultureInfos = new()
            {
                ru, en
            };

            return cultureInfos;
        }

        #endregion
    }
}
