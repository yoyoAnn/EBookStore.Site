using EBookStore.Site.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Site.Models
{
    public interface IPublisher
    {
        /// <summary>
        /// 檢查資料庫裡是否有相同名稱的廠商
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsPublisherNameExists(string name);


        /// <summary>
        /// 新增出版商
        /// </summary>
        /// <param name="dto"></param>
        void CreatePublisher(PublishersDto dto);
    }
}
