using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
    public static class BookHelper
    {
        public static int GetWorksheetNumber(string category)
        {
            switch (category)
            {
                case "文學小說":
                    return 1;
                case "商業理財":
                    return 2;
                case "藝術設計":
                    return 3;
                case "人文社科":
                    return 4;
                case "心理勵志":
                    return 5;
                case "宗教命理":
                    return 6;
                case "自然科普":
                    return 7;
                case "醫療保健":
                    return 8;
                case "飲食":
                    return 9;
                case "生活風格":
                    return 10;
                default:
                    throw new ArgumentException("Invalid category");
            }
        }

 
    }
}