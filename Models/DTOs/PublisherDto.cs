using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.DTOs
{
    public class PublishersDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Email { get; set; } = "";
    }

    public static class PublishersExt
    {
        public static PublishersDto ToDto(this PublishersVM vm)
        {
            var dto = new PublishersDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Address = vm.Address,
                Phone = vm.Phone,
                Email = vm.Email,
            };
            dto.Address= SetEmptyStringIfNull(dto.Address);
            dto.Phone = SetEmptyStringIfNull(dto.Phone);
            dto.Email = SetEmptyStringIfNull(dto.Email);

            return dto;
        }

        private static string SetEmptyStringIfNull(string value)
        {
            if (value == null)
            {
                return "";
               
            }
            return value;
        }
    }
}