using System;
using System.Collections.Generic;

namespace products_api_istest.Models
{
    public partial class PersonCreditCard
    {
        public int BusinessEntityId { get; set; }
        public int CreditCardId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Person BusinessEntity { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
