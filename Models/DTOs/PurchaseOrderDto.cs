using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id{ get; set; }

        public int BookId{ get; set; }

        public int PublisherId{ get; set; }

        public int Qty{ get; set; }

        public string Detail{ get; set; }

        public Decimal PurchasePrice{ get; set; }
    }
}