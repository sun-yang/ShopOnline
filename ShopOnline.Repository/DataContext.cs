namespace ShopOnline.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
               .HasKey(ci => new { ci.UserId, ci.ProductId});

            modelBuilder.Entity<OrderItem>()
               .HasKey(oi => new {oi.OrderId, oi.ProductId });

            modelBuilder.Entity<Product>().HasData(
                        new Product
                        {
                            Id = 1,
                            Name = "The 156-Storey Treehouse",
                            Description = "Andy and Terry are celebrating Christmas in their 156-storey treehouse which now has 13 new storeys, including an aquarium wonderland, a wishing well, a world record breaking level, a TV quiz show hosted by Quizzy the quizzical quizbot, a lost property office, a lost sausage office, a super-stinky stuff level and the amazing mind-reading sandwich-making machine, " +
                            "which makes the perfect amazing sandwich for you - every single time! Well, what are you waiting for? Come on up!",
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/513mIP6TD5L._SY344_BO1,204,203,200_QL70_ML2_.jpg",
                            Price = 11.99m,
                            CategoryId = 1
                        },
                        new Product
                        {
                            Id = 2,
                            Name = "Bluey: Let's Get Spooky! A Magnet Book",
                            Description = "Based on the hit ABC KIDS TV show! BOO! It's time to get spooky! Use the magnets to take Bluey, Bingo and their friends on a fun-filled night-time adventure. A gorgeous magnet book for kids of all ages. Bluey is an award - winning preschool show about Bluey, a blue heeler pup, and her family.Airing on ABC KIDS, the show has amassed legions of dedicated fans and hugely popular ranges of books, toys, clothes, games and more.",
                            ImageUrl = "https://www.dymocks.com.au/Pages/ImageHandler.ashx?q=9781761046285&w=&h=570",
                            Price = 9.9m,
                            CategoryId = 2
                        },
                         new Product
                         {
                             Id = 3,
                             Name = "Cheesy Weird!",
                             Description = "Say Cheese! Not only is it school photo day, but there are try-outs for an ice-cream ad! Can Weir and his friends score the starring roles? Or will their TV dreams melt away?! It won’t be easy ... but it will be funny!",
                             ImageUrl = "https://www.dymocks.com.au/Pages/ImageHandler.ashx?q=9781761127403&w=&h=570",
                             Price = 12.59m,
                             CategoryId = 3
                         }
                        );
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Fiction",
                    Url = "Fiction"
                },
                new Category
                {
                    Id = 2,
                    Name = "Non-Fiction",
                    Url = "Non-Fiction"
                },
                new Category
                {
                    Id = 3,
                    Name = "Learning",
                    Url = "Learning"
                }
                );
        }
    }
}
