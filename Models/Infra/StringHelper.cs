using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
    public static class StringHelper
    {
        public static string SetEmptyStringIfNull(string value)
        {
            if (value == null)
            {
                return "";

            }
            return value;
        }
    }
}