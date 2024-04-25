using AdamController.Services.Interfaces;
using System;
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

        public List<CultureInfo> SupportAppCultures { get { return GetSupportAppCultures(); } }

        public CultureInfo CurrentAppCulture {  get; private set; }

        #endregion

        #region Public methods

        public void ChangeAppCulture(CultureInfo culture)
        {
            string resourceName = $"pack://application:,,,/AdamController.Core;component/LocalizationDictionary/{culture.TwoLetterISOLanguageName}.xaml";
            Uri uri = new(resourceName);
            ResourceDictionary resources = new()
            {
                Source = uri
            };

            RemoveLoadedDictonary();
   
            Application.Current.Resources.MergedDictionaries.Add(resources);
            UpdateCurrentCulture(culture);
        }


        public void Dispose()
        {

        }

        #endregion

        #region Private method

        private void UpdateCurrentCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CurrentAppCulture = culture;
        }

        private void RemoveLoadedDictonary()
        {
            List<CultureInfo> supportedCultures = SupportAppCultures;
            ResourceDictionary currentResourceDictionary = null;

            foreach (var culture in supportedCultures)
            {
                string resourceName = $"pack://application:,,,/AdamController.Core;component/LocalizationDictionary/{culture.TwoLetterISOLanguageName}.xaml"; 
                currentResourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x?.Source?.OriginalString == resourceName);
            }

            if (currentResourceDictionary == null || currentResourceDictionary?.MergedDictionaries.Count == 0) 
                return;
           
            foreach (ResourceDictionary dictionary in currentResourceDictionary.MergedDictionaries)
                Application.Current.Resources.MergedDictionaries.Remove(dictionary);
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
