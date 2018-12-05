using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDBTestConsole
{
    public class PurchaseFoodOrBeverage : IInteraction
    {
        public decimal unitPrice { get; set; }
        public decimal totalPrice { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
    }
}
