using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Health_Consultation_Portal.Migrations
{
    /// <inheritdoc />
    public partial class PatientBloodType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Patients");
        }
    }
}
