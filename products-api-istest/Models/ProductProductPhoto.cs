﻿using System;
using System.Collections.Generic;

namespace products_api_istest.Models
{
    public partial class ProductProductPhoto
    {
        public int ProductId { get; set; }
        public int ProductPhotoId { get; set; }
        public bool Primary { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Product Product { get; set; }
        public ProductPhoto ProductPhoto { get; set; }
    }
}
