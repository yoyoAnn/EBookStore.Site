using EBookStore.Site.Models.BooksViewsModel;
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
        /// <summary>
        /// 創建訂貨訂單
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="bookid"></param>
        /// <param name="publisherid"></param>
        void Create(PurchaseOrderDapperVM vm);

        void Delete(int id);
        /// <summary>
        /// 確認送出訂單，會把數量加進書本庫存裡
        /// </summary>
        void ConfirmOrder(int POid,int bookid);
      
        /// <summary>
        /// 取得所有進貨訂單
        /// </summary>
        /// <returns></returns>
        List<PurchaseOrderDapperVM> GetAll();
    }
}
