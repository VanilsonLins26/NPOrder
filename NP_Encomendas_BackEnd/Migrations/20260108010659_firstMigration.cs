using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NP_Encomendas_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartHeaders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeliverTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PromotionPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Customizable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnityPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CartHeaderId = table.Column<int>(type: "int", nullable: false),
                    ImageUrls = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Customize = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnityPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrls = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Customize = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Active", "CreateTime", "Customizable", "Description", "ImageUrl", "Name", "Price", "PromotionPrice", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Bolo personalizado, serve 20 fatias.", "/img/bolo1.jpg", "Bolo Temático 1 Andar", 150.00m, null, "Unidade" },
                    { 2, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Brigadeiro com chocolate belga.", "/img/brigadeiro.jpg", "Cento de Brigadeiro Gourmet", 120.00m, 110.00m, "Cento" },
                    { 3, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Sabor do dia, consulte.", "/img/bolo_pote.jpg", "Bolo de Pote (Pronta Entrega)", 12.00m, null, "Unidade" },
                    { 4, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Coxinhas crocantes e sequinhas.", "/img/coxinha.jpg", "Cento de Coxinha (Frango)", 90.00m, null, "Cento" },
                    { 5, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Massa podre com recheio cremoso.", "/img/torta_frango.jpg", "Torta Salgada de Frango (Kg)", 75.00m, null, "Kg" },
                    { 6, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Kit com 1 mini-bolo, 50 salgados, 20 doces.", "/img/kit_festa.jpg", "Kit Festa (10 pessoas)", 180.00m, null, "Kit" },
                    { 7, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Empadão cremoso de palmito.", "/img/empadao.jpg", "Empadão de Palmito (Kg)", 80.00m, null, "Kg" },
                    { 8, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Bordado ponto cruz personalizado com nome.", "/img/toalha.jpg", "Toalha de Mesa Bordada", 250.00m, null, "Unidade" },
                    { 9, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vela artesanal, 200g.", "/img/vela.jpg", "Vela Aromática (Lavanda)", 45.00m, null, "Unidade" },
                    { 10, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Polvo de crochê, cores a escolher.", "/img/polvo.jpg", "Amigurumi (Polvo)", 90.00m, null, "Unidade" },
                    { 11, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cesta completa com pães, frutas e frios.", "/img/cesta_cafe.jpg", "Cesta de Café da Manhã Standard", 180.00m, null, "Unidade" },
                    { 12, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Seleção de queijos e um vinho tinto.", "/img/cesta_vinho.jpg", "Cesta de Queijos e Vinhos", 260.00m, null, "Unidade" },
                    { 13, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Taxa de montagem. Adicione os itens avulsos no pedido.", "/img/cesta_vazia.jpg", "Monte sua Cesta (Serviço)", 50.00m, null, "Unidade" },
                    { 14, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Seleção de chocolates finos.", "/img/cesta_choco.jpg", "Cesta de Chocolates", 150.00m, null, "Unidade" },
                    { 15, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Buquê com 6 rosas.", "/img/buque.jpg", "Buquê de Rosas (Pequeno)", 70.00m, null, "Unidade" },
                    { 16, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Topo de bolo em papel lamicote. Informe nome e idade.", "/img/topo_bolo.jpg", "Topo de Bolo Personalizado", 35.00m, null, "Unidade" },
                    { 17, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Convite em papel texturizado, com laço.", "/img/convite.jpg", "Convite de Casamento (Unidade)", 8.50m, null, "Unidade" },
                    { 18, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cartela de adesivos tema 'Planner'.", "/img/adesivos.jpg", "Kit de Adesivos (Pronta Entrega)", 15.00m, null, "Unidade" },
                    { 19, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Caderno A5 com capa dura personalizada (envie a arte).", "/img/caderno.jpg", "Caderno Personalizado (Capa)", 55.00m, null, "Unidade" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartHeaders");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
