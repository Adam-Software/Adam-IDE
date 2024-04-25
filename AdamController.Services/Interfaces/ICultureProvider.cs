using System;
using System.Collections.Generic;
using System.Globalization;

namespace AdamController.Services.Interfaces
{
    public interface ICultureProvider : IDisposable
    {
        #region Public fields

        public List<CultureInfo> SupportAppCultures { get; }
        public CultureInfo CurrentAppCulture { get; }

        #endregion

        #region Public methods

        public void ChangeAppCulture(CultureInfo culture);

        #endregion
    }
}
