using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewModels
{
    public static class EmployeeExts
    {
        public static EmployeeIndexVM ToIndexVM(this Employee entity)
        {
            return new EmployeeIndexVM { 
                Id = entity.Id,
                RoleName = entity.Role.Name,
                Account = entity.Account,
                Name = entity.Name,
                Gender = entity.Gender,
                Phone = entity.Phone,
                Email = entity.Email,
                CreatedTime = entity.CreatedTime
            };

        }

        public static EmployeeCreateVM ToCreateVM(this Employee entity)
        {
            return new EmployeeCreateVM
            {
                Id = entity.Id,
                //RoleName = entity.Role.Name,
                RoleId = entity.RoleId,
                Account = entity.Account,
                Name = entity.Name,
                Gender = entity.Gender,
                Phone = entity.Phone,
                Email = entity.Email,
                CreatedTime = entity.CreatedTime
            };           
        }

        public static Employee ToEntity(this EmployeeCreateVM vm)
        {
            return new Employee
            {
                Id = vm.Id,
                //RoleName = vm.Role.Name,
                RoleId = vm.RoleId,
                Account = vm.Account,
                Name = vm.Name,
                Gender = vm.Gender,
                Phone = vm.Phone,
                Email = vm.Email,
                CreatedTime = vm.CreatedTime
            };
        }
    }
}