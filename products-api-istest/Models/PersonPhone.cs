﻿using System;
using System.Collections.Generic;

namespace products_api_istest.Models
{
    public partial class PersonPhone
    {
        public int BusinessEntityId { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberTypeId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Person BusinessEntity { get; set; }
        public PhoneNumberType PhoneNumberType { get; set; }
    }
}
