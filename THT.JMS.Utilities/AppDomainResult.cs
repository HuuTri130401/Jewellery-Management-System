﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THT.JMS.Utilities
{
    public class AppDomainResult
    {
        public AppDomainResult()
        {
        }
        public bool success { get; set; } = false;
        public object data { get; set; }
        public int resultCode { get; set; }
        public string resultMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
