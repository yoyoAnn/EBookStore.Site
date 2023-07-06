using EBookStore.Site.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Site.Models
{
    public interface IPurchaseOrders
    {
        void Create(PurchaseOrderDto dto);

        void Delete(int id);


    }
}
