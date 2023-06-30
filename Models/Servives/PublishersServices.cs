using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Servives
{
    public class PublishersServices : IPublisher
    {
        private readonly AppDbContext _db;

        public PublishersServices(AppDbContext db)
        {
            _db = db;
        }
        public void CreatePublisher(PublishersDto dto)
        {
            var publisher = new Publisher
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _db.Publishers.Add(publisher);
            _db.SaveChanges();
        }
        public bool IsPublisherNameExists(string name)
        {
            return _db.Publishers.Any(p => p.Name == name);
        }
    }
}