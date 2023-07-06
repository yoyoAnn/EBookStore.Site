using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra.DapperRepository
{
    public class PurchaseOrdersDapper : IPurchaseOrders
    {
        private readonly IDbConnection _connection;
        private readonly AppDbContext _db;

        public PurchaseOrdersDapper(AppDbContext db)
        {
            _db = db;
            string connStr = "data source=.;initial catalog=EBookStore;user id=ebookLogin;password=123;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

            _connection = new SqlConnection(connStr);
        }


        public void Create(PurchaseOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}