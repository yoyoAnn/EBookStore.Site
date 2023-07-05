using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
    public static class UserExts
    {
        public static UserIndexVM ToIndexVM(this User entity)
        {
            return new UserIndexVM
            {
                Id = entity.Id,
                Account = entity.Account,
                Name = entity.Name,
                Gender = (bool)entity.Gender,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address,
                Photo = entity.Photo,
                CreatedTime = entity.CreatedTime,
                IsConfirmed = entity.IsConfirmed
            };

        }

        public static UserCreateVM ToCreateVM(this User entity)
        {
            return new UserCreateVM
            {
                Id = entity.Id,
                Account = entity.Account,
                Password = entity.Password,
                Name = entity.Name,
                Gender = (bool)entity.Gender,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address,
                Photo = entity.Photo,
                CreatedTime = entity.CreatedTime,
            };
        }

        public static User ToEntity(this UserCreateVM vm)
        {
            return new User
            {
                Id = vm.Id,
                Account = vm.Account,
                Password = vm.Password,
                Name = vm.Name,
                Gender = (bool)vm.Gender,
                Phone = vm.Phone,
                Email = vm.Email,
                Address = vm.Address,
                Photo = vm.Photo,
                CreatedTime = vm.CreatedTime,
            };
        }
    }

}