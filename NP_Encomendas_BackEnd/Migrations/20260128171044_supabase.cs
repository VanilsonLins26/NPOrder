using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NP_Encomendas_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class supabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Complement = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Sent = table.Column<bool>(type: "boolean", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "text", nullable: true),
                    Customizable = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false),
                    DeliverTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnityPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CartHeaderId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    PromotionalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    InitialDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FinalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnityPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    NetReceivedAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    FeeAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    Installments = table.Column<int>(type: "integer", nullable: true),
                    StatusDetail = table.Column<string>(type: "text", nullable: true),
                    PaymentMethodId = table.Column<string>(type: "text", nullable: true),
                    PaymentTypeId = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateApproved = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MoneyReleaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PaymentUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Active", "CreateTime", "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo 10 de cada sabor. Sabores: Calabresa, Misto, Carne do sol, Frango c/queijo, Queijo.", "https://localhost:7023/images/products/miniPizza.png", "Mini Pizza (10 unidades)", 15.00m, "Pacote" },
                    { 2, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo 25 de cada sabor. Sabores: Doce de leite, Limão, Brigadeiro, M&M, Frango, Carne do sol.", "https://localhost:7023/images/products/miniEmpada.png", "Mini Empada (25 unidades)", 30.00m, "Pacote" },
                    { 3, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo 25 de cada sabor. Sabores: Queijo, Carne moída, Frango, Calabresa c/queijo, Misto, Carne do sol, Chocolate, Nutella, Queijo com goiaba, Banana com canela.", "https://localhost:7023/images/products/miniPasteis.png", "Mini Pastéis (25 unidades)", 27.50m, "Pacote" },
                    { 4, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pavê de leite condensado, biscoito e coberto com uma ganache de chocolate. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/pave.png", "Travessa de Pavê", 70.00m, "Unidade" },
                    { 5, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Mousse com pedacinhos de abacaxi e biscoito. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/paveAbacaxi.png", "Pavê de Abacaxi", 75.00m, "Unidade" },
                    { 6, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Creme de leite condensado e doce de goiaba. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/paveLeiteGoiaba.png", "Pavê de Leite e Goiaba", 65.00m, "Unidade" },
                    { 7, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Brigadeiro Colorido ou preto, Nutella, Tradicional.", "https://localhost:7023/images/products/miniBrownie.png", "Mini Brownie (25 unidades)", 30.00m, "Pacote" },
                    { 8, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Brigadeiro, Cenoura com chocolate, Limão com doce de leite.", "https://localhost:7023/images/products/miniBolo.png", "Mini Bolo (20 unidades)", 25.00m, "Pacote" },
                    { 9, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Goiabinha, Nutella, Gotas de chocolate, M&M.", "https://localhost:7023/images/products/miniBiscoitosFesta.png", "Mini Biscoitos de Festa (100 unidades)", 50.00m, "Cento" },
                    { 10, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Serve de 10 a 15 pessoas. Opções: Torta fria de frango ou Torta fria de frango c/goiabada.", "https://localhost:7023/images/products/paoAmericano.png", "Pão Americano", 80.00m, "Unidade" },
                    { 11, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Serve de 10 a 15 pessoas. Sabores: Queijo e presunto, Frango e queijo, Carne do sol e queijo.", "https://localhost:7023/images/products/paoMetro.png", "Pão de Metro", 45.00m, "Unidade" },
                    { 12, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Serve de 8 a 10 pessoas. Sabores: Calabresa e queijo, Presunto e queijo, Frango e requeijão, Carne do sol e queijo.", "https://localhost:7023/images/products/paoTranca.png", "Pão Trança", 45.00m, "Unidade" },
                    { 13, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Clássico pudim de leite. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/pudim.png", "Pudim de Leite", 60.00m, "Unidade" },
                    { 14, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Fundo de biscoito e um mousse de limão, coberto com chantily e raspas de limão. Serve 12 a 15 pessoas.", "https://localhost:7023/images/products/tortaLimao.png", "Torta de Limão", 70.00m, "Unidade" },
                    { 15, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Fundo de biscoito de chocolate, mousse de maracujá e um delicioso mousse de chocolate. Serve 12 a 15 pessoas.", "https://localhost:7023/images/products/tortaMaracujaChocolate.png", "Torta de Maracujá com Chocolate", 80.00m, "Unidade" },
                    { 16, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Empadão de frango cremoso. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoFrango.png", "Empadão de Frango", 50.00m, "Unidade" },
                    { 17, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Empadão de carne do sol e requeijão. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoCarneSol.png", "Empadão de Carne do Sol", 65.00m, "Unidade" },
                    { 18, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Empadão de camarão cremoso e requeijão. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoCamarao.png", "Empadão de Camarão Cremoso", 95.00m, "Unidade" },
                    { 19, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão árabe, alface, queijo, presunto e requeijão.", "https://localhost:7023/images/products/miniPaoArabe.png", "Mini Pão Árabe (25 unidades)", 37.50m, "Pacote" },
                    { 20, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite e creme de frango.", "https://localhost:7023/images/products/miniGaloFrio.png", "Mini Galo Frio (10 unidades)", 20.00m, "Pacote" },
                    { 21, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Queijo ou Misto.", "https://localhost:7023/images/products/miniCroissant.png", "Mini Croissant (10 unidades)", 35.00m, "Pacote" },
                    { 22, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo de 25 de cada sabor. Sabores: Coxinha, Bolinha, Carne, Misto, Carne do sol, Pastel de carne/queijo/misto.", "https://localhost:7023/images/products/centosalgado.png", "Cento de Salgados", 45.00m, "Cento" },
                    { 23, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo de 25 de cada sabor. Sabores: Misto, Queijo, Carne do sol, Frango, Pão pizza.", "https://localhost:7023/images/products/salgadoforno.png", "Cento de Salgados de Forno", 55.00m, "Cento" },
                    { 24, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Queijo, Queijo com orégano, Alho.", "https://localhost:7023/images/products/centoBiscoitinhoFornoFesta.png", "Cento de Biscoitinho de Forno", 50.00m, "Cento" },
                    { 25, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite, carne artesanal, alface, tomate, queijo, maionese e ketchup.", "https://localhost:7023/images/products/miniHamburguer.png", "Mini Hambúrguer (10 unidades)", 35.00m, "Pacote" },
                    { 26, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite, molho de tomate, batata palha, maionese e ketchup.", "https://localhost:7023/images/products/miniCachorroQuente.png", "Mini Cachorro Quente (10 unidades)", 20.00m, "Pacote" },
                    { 27, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo 10 de cada sabor. Sabores: Salsicha, Queijo, Misto, Mistão, Frango, Carne do sol, Pizza, Calabresa.", "https://localhost:7023/images/products/miniSalgadoForno.png", "Mini Salgados de Forno (10 unidades)", 12.50m, "Pacote" },
                    { 28, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cheesecake com geleia de morango. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/cheesecake.png", "Cheesecake de Morango", 95.00m, "Unidade" },
                    { 29, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Guirlanda de brownie com brigadeiro e morangos. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/guirlandaBrownie.png", "Guirlanda de Brownie", 75.00m, "Unidade" },
                    { 30, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Fundo de biscoito, bananas, doce de leite e uma camada de chantily canela e chocolate. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/banoffee.png", "Banoffee", 80.00m, "Unidade" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ProductId",
                table: "Promotions",
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
                name: "NotificationQueues");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
