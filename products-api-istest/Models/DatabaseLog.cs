using System;
using System.Collections.Generic;

namespace products_api_istest.Models
{
    public partial class DatabaseLog
    {
        public int DatabaseLogId { get; set; }
        public DateTime PostTime { get; set; }
        public string Tsql { get; set; }
        public string XmlEvent { get; set; }
    }
}
