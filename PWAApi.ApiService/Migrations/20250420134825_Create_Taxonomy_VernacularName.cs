using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApi.Migrations
{
    /// <inheritdoc />
    public partial class Create_Taxonomy_VernacularName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taxonomy",
                columns: table => new
                {
                    TaxonKey = table.Column<int>(type: "int", nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptedTaxonKey = table.Column<int>(type: "int", nullable: false),
                    AcceptedScientificName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfOccurences = table.Column<int>(type: "int", nullable: false),
                    TaxonRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxonomicStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kingdom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KingdomKey = table.Column<int>(type: "int", nullable: true),
                    Phylum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhylumKey = table.Column<int>(type: "int", nullable: true),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassKey = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderKey = table.Column<int>(type: "int", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyKey = table.Column<int>(type: "int", nullable: true),
                    Genus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenusKey = table.Column<int>(type: "int", nullable: true),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeciesKey = table.Column<int>(type: "int", nullable: true),
                    IUCNRedListCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxonomy", x => x.TaxonKey);
                });

            migrationBuilder.CreateTable(
                name: "VernacularNames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxonKey = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LifeStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPreferredName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VernacularNames", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VernacularNames_Taxonomy_TaxonKey",
                        column: x => x.TaxonKey,
                        principalTable: "Taxonomy",
                        principalColumn: "TaxonKey");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VernacularNames_TaxonKey",
                table: "VernacularNames",
                column: "TaxonKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VernacularNames");

            migrationBuilder.DropTable(
                name: "Taxonomy");
        }
    }
}
