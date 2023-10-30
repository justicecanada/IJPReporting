using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting.Code
{
    [Serializable]
    public class CsvResultSet
    {

        public string data;

        public string fileName;
        

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}