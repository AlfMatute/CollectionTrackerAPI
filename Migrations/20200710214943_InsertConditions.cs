using Microsoft.EntityFrameworkCore.Migrations;

namespace CollectionTrackerAPI.Migrations
{
    public partial class InsertConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Conditions (Description, Active) VALUES ('New', 1), ('Renewed', 1), ('Rental', 1), ('Used - Like new or Open Box', 1), ('Used - Very Good', 1), ('Used - Good', 1), ('Used - Acceptable', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Conditions  WHERE Description IN ('New', 'Renewed', 'Rental', 'Used - Like new or Open Box', 'Used - Very Good', 'Used - Good', 'Used - Acceptable') AND Active = 1");
        }
    }
}
