using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechLife.CSDLMoiTruong.Common
{
    public static class SystemTempData
    {
        public static void AddTempData(this ITempDataDictionary tempData, string typeData, string data)
        {
            tempData[typeData] = data;
        }
        public static string GetTempData(this ITempDataDictionary tempData, string typeData)
        {
            return tempData[typeData] as string;
        }
        public static void CreateTempData(this ITempDataDictionary tempData, string typeData)
        {
            if (!tempData.ContainsKey(typeData))
            {
                tempData[typeData] = null;
            }
        }
    }
}
