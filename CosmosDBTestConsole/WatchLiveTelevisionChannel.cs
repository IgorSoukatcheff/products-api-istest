using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDBTestConsole
{
    public class WatchLiveTelevisionChannel : IInteraction
    {
        public string channelName { get; set; }
        public int minutesViewed { get; set; }
        public string type { get; set; }
    }
}
