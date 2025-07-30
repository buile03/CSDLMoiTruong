﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Common.Result
{
    public class LoginResult<T>
    {
        public bool IsSuccessed { get; set; }
        public bool IsLockout { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
    public class ApiLoginResult<T> : LoginResult<T>
    {
        public ApiLoginResult(T result, string message = "", bool isSuccessed = true, bool isLockout = false)
        {
            IsSuccessed = isSuccessed;
            Message = message;
            IsLockout = isLockout;
            ResultObj = result;
        }
    }

}
