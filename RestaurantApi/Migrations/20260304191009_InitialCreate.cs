using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rango_reserva",
                columns: table => new
                {
                    id_rango_reserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    cupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__rango_re__E110E7BBC5D238EB", x => x.id_rango_reserva);
                });

            migrationBuilder.CreateTable(
                name: "reserva",
                columns: table => new
                {
                    cod_reserva = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nombre_persona = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellido_persona = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    dni = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    mail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    celular = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    fecha_reserva = table.Column<DateTime>(type: "datetime", nullable: false),
                    id_rango_reserva = table.Column<int>(type: "int", nullable: false),
                    cantidad_personas = table.Column<int>(type: "int", nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reserva__12EE26860D1B7BCA", x => x.cod_reserva);
                    table.ForeignKey(
                        name: "FK__reserva__id_rang__398D8EEE",
                        column: x => x.id_rango_reserva,
                        principalTable: "rango_reserva",
                        principalColumn: "id_rango_reserva");
                });

            migrationBuilder.CreateIndex(
                name: "IX_reserva_id_rango_reserva",
                table: "reserva",
                column: "id_rango_reserva");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reserva");

            migrationBuilder.DropTable(
                name: "rango_reserva");
        }
    }
}
