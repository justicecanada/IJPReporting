﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting
{
    [Serializable]
    public class DataTableResultSet
    {
        /// <summary>Array of records. Each element of the array is itself an array of columns</summary>
        public List<List<string>> data = new List<List<string>>();

        /// <summary>value of draw parameter sent by client</summary>
        public int draw;

        /// <summary>filtered record count</summary>
        public int recordsFiltered;

        /// <summary>total record count in resultset</summary>
        public int recordsTotal;

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    [Serializable]
    public class DataTableResultError : DataTableResultSet
    {
        public string error;
    }

}