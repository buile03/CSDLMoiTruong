using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TechLife.CSDLMoiTruong.Common.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechLife.CSDLMoiTruong.Common
{
    public static class SystemAlerts
    {
        public static Result.Result GetAlert(this ITempDataDictionary tempData)
        {
            CreateAlertTempData(tempData);
            return DeserializeAlerts(tempData[SystemConstants.Alerts] as string);
        }

        public static void CreateAlertTempData(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(SystemConstants.Alerts))
            {
                tempData[SystemConstants.Alerts] = null;
            }

        }
        public static void AddAlert(this ITempDataDictionary tempData, Result.Result alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }
            tempData[SystemConstants.Alerts] = SerializeAlerts(alert);
        }
        public static string SerializeAlerts(Result.Result tempData)
        {
            return JsonConvert.SerializeObject(tempData);
        }
        public static Result.Result DeserializeAlerts(string tempData)
        {
            if (tempData != null)
            {
                if (tempData.Length == 0)
                {
                    return Result.Result.Success();
                }
                return JsonConvert.DeserializeObject<Result.Result>(tempData);
            }
            else
            {
                return null;
            }
        }
    }
}
