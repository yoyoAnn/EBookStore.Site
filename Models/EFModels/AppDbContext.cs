using EBookStore.Site.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EBookStore.Site.Models.EFModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<BookImage> BookImages { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CustomerServiceMail> CustomerServiceMails { get; set; }
        public virtual DbSet<EBookOrder> EBookOrders { get; set; }
        public virtual DbSet<EBook> EBooks { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ProblemType> ProblemTypes { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<RepliedMail> RepliedMails { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; }
        public virtual DbSet<UserArticleCollection> UserArticleCollections { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Writer> Writers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.BookAuthors)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookImage>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.ISBN)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.EISBN)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Articles)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookAuthors)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookImages)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.EBooks)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.PurchaseOrders)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerServiceMail>()
                .Property(e => e.UserAccount)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerServiceMail>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerServiceMail>()
                .HasMany(e => e.RepliedMails)
                .WithRequired(e => e.CustomerServiceMail)
                .HasForeignKey(e => e.CSId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EBookOrder>()
                .Property(e => e.Payment)
                .HasPrecision(18, 0);

            modelBuilder.Entity<EBook>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<EBook>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<EBook>()
                .HasMany(e => e.EBookOrders)
                .WithRequired(e => e.EBook)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.ReceiverPhone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.TaxIdNum)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.VehicleNum)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.ShippingFee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalPayment)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.RolePermissions)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProblemType>()
                .HasMany(e => e.CustomerServiceMails)
                .WithRequired(e => e.ProblemType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publisher>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Publisher>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Publisher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.PurchaseOrders)
                .WithRequired(e => e.Publisher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RolePermissions)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.ConfirmCode)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.EBookOrders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Writer>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<Writer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Writer>()
                .HasMany(e => e.Articles)
                .WithRequired(e => e.Writer)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.BooksViewsModel.BooksDapperVM> BooksDapperVMs { get; set; }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.ViewsModel.BooksVM> BooksVMs { get; set; }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.ViewModels.EmployeeIndexVM> EmployeeIndexVms { get; set; }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.ViewModels.EmployeeCreateVM> EmployeeCreateVMs { get; set; }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.ViewModels.UserIndexVM> UserIndexVMs { get; set; }

        public System.Data.Entity.DbSet<EBookStore.Site.Models.Infra.OrdersItemDapperVM> OrdersItemDapperVMs { get; set; }
    }
}

