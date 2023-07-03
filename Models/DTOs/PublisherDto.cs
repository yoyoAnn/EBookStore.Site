using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using EBookStore.Site.Models.Infra;

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
            dto.Address= StringHelper.SetEmptyStringIfNull(dto.Address);
            dto.Phone = StringHelper.SetEmptyStringIfNull(dto.Phone);
            dto.Email = StringHelper.SetEmptyStringIfNull(dto.Email);

            return dto;
        }       
    }
}