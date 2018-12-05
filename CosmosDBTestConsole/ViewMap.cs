using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDBTestConsole
{
    public class ViewMap : IInteraction
    {
        public int minutesViewed { get; set; }
        public string type { get; set; }
    }
}
