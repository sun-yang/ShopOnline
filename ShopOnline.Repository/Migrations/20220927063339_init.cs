using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnline.Repository.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 1, "Andy and Terry are celebrating Christmas in their 156-storey treehouse which now has 13 new storeys, including an aquarium wonderland, a wishing well, a world record breaking level, a TV quiz show hosted by Quizzy the quizzical quizbot, a lost property office, a lost sausage office, a super-stinky stuff level and the amazing mind-reading sandwich-making machine, which makes the perfect amazing sandwich for you - every single time! Well, what are you waiting for? Come on up!", "https://static.wikia.nocookie.net/storey-treehouse/images/9/9b/The_156_Storey_Treehouse_-_Australian_Edition.jpg/revision/latest?cb=20220621081735", "The 156-Storey Treehouse", 0m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 2, "Based on the hit ABC KIDS TV show! BOO! It's time to get spooky! Use the magnets to take Bluey, Bingo and their friends on a fun-filled night-time adventure. A gorgeous magnet book for kids of all ages. Bluey is an award - winning preschool show about Bluey, a blue heeler pup, and her family.Airing on ABC KIDS, the show has amassed legions of dedicated fans and hugely popular ranges of books, toys, clothes, games and more.", "https://www.dymocks.com.au/Pages/ImageHandler.ashx?q=9781761046285&w=&h=570", "Bluey: Let's Get Spooky! A Magnet Book", 0m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 3, "Say Cheese! Not only is it school photo day, but there are try-outs for an ice-cream ad! Can Weir and his friends score the starring roles? Or will their TV dreams melt away?! It won’t be easy ... but it will be funny!", "https://www.dymocks.com.au/Pages/ImageHandler.ashx?q=9781761127403&w=&h=570", "Cheesy Weird!", 0m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
