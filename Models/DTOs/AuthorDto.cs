using DocumentFormat.OpenXml.Wordprocessing;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Profile { get; set; }
    }

    public static class AuthorExt
    {
        public static Author ToEntity(this AuthorDto dto)
        {
            var entity = new Author
            {
                Id = dto.Id,
                Name = dto.Name,
                Photo = dto.Photo,
                Profile = dto.Profile
            };

            entity.Photo = StringHelper.SetEmptyStringIfNull(dto.Photo);
            entity.Profile = StringHelper.SetEmptyStringIfNull(dto.Profile);

            return entity;
        }
    }
}