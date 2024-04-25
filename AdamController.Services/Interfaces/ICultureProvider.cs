

using System;
using System.Collections.Generic;
using System.Globalization;

namespace AdamController.Services.Interfaces
{
    public interface ICultureProvider : IDisposable
    {
        public List<CultureInfo> AppSupportCultureInfos { get; }
        public void LoadCultureInfoDictonary(CultureInfo culture);
        public CultureInfo CurrentAppCultureInfo { get; }
    }
}
