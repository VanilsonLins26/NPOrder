using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NP_Encomendas_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class attProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Mínimo 10 de cada sabor. Sabores: Calabresa, Misto, Carne do sol, Frango c/queijo, Queijo.", "https://localhost:7023/images/products/miniPizza.png", "Mini Pizza (10 unidades)", 15.00m, "Pacote" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "PromotionPrice", "UnitOfMeasure" },
                values: new object[] { true, "Mínimo 25 de cada sabor. Sabores: Doce de leite, Limão, Brigadeiro, M&M, Frango, Carne do sol.", "https://localhost:7023/images/products/miniEmpada.png", "Mini Empada (25 unidades)", 30.00m, null, "Pacote" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { true, "Mínimo 25 de cada sabor. Sabores: Queijo, Carne moída, Frango, Calabresa c/queijo, Misto, Carne do sol, Chocolate, Nutella, Queijo com goiaba, Banana com canela.", "https://localhost:7023/images/products/miniPasteis.png", "Mini Pastéis (25 unidades)", 27.50m, "Pacote" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Pavê de leite condensado, biscoito e coberto com uma ganache de chocolate. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/pave.png", "Travessa de Pavê", 70.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Name", "UnitOfMeasure" },
                values: new object[] { "Mousse com pedacinhos de abacaxi e biscoito. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/paveAbacaxi.png", "Pavê de Abacaxi", "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { false, "Creme de leite condensado e doce de goiaba. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/paveLeiteGoiaba.png", "Pavê de Leite e Goiaba", 65.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { true, "Sabores: Brigadeiro Colorido ou preto, Nutella, Tradicional.", "https://localhost:7023/images/products/miniBrownie.png", "Mini Brownie (25 unidades)", 30.00m, "Pacote" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Sabores: Brigadeiro, Cenoura com chocolate, Limão com doce de leite.", "https://localhost:7023/images/products/miniBolo.png", "Mini Bolo (20 unidades)", 25.00m, "Pacote" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { true, "Sabores: Goiabinha, Nutella, Gotas de chocolate, M&M.", "https://localhost:7023/images/products/miniBiscoitosFesta.png", "Mini Biscoitos de Festa (100 unidades)", 50.00m, "Cento" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Serve de 10 a 15 pessoas. Opções: Torta fria de frango ou Torta fria de frango c/goiabada.", "https://localhost:7023/images/products/paoAmericano.png", "Pão Americano", 80.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { true, "Serve de 10 a 15 pessoas. Sabores: Queijo e presunto, Frango e queijo, Carne do sol e queijo.", "https://localhost:7023/images/products/paoMetro.png", "Pão de Metro", 45.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { true, "Serve de 8 a 10 pessoas. Sabores: Calabresa e queijo, Presunto e queijo, Frango e requeijão, Carne do sol e queijo.", "https://localhost:7023/images/products/paoTranca.png", "Pão Trança", 45.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { false, "Clássico pudim de leite. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/pudim.png", "Pudim de Leite", 60.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Fundo de biscoito e um mousse de limão, coberto com chantily e raspas de limão. Serve 12 a 15 pessoas.", "https://localhost:7023/images/products/tortaLimao.png", "Torta de Limão", 70.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Fundo de biscoito de chocolate, mousse de maracujá e um delicioso mousse de chocolate. Serve 12 a 15 pessoas.", "https://localhost:7023/images/products/tortaMaracujaChocolate.png", "Torta de Maracujá com Chocolate", 80.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { false, "Empadão de frango cremoso. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoFrango.png", "Empadão de Frango", 50.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { false, "Empadão de carne do sol e requeijão. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoCarneSol.png", "Empadão de Carne do Sol", 65.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Empadão de camarão cremoso e requeijão. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/empadaoCamarao.png", "Empadão de Camarão Cremoso", 95.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { false, "Pão árabe, alface, queijo, presunto e requeijão.", "https://localhost:7023/images/products/miniPaoArabe.png", "Mini Pão Árabe (25 unidades)", 37.50m, "Pacote" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Active", "CreateTime", "Customizable", "Description", "ImageUrl", "Name", "Price", "PromotionPrice", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 20, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite e creme de frango.", "https://localhost:7023/images/products/miniGaloFrio.png", "Mini Galo Frio (10 unidades)", 20.00m, null, "Pacote" },
                    { 21, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Queijo ou Misto.", "https://localhost:7023/images/products/miniCroissant.png", "Mini Croissant (10 unidades)", 35.00m, null, "Pacote" },
                    { 22, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo de 25 de cada sabor. Sabores: Coxinha, Bolinha, Carne, Misto, Carne do sol, Pastel de carne/queijo/misto.", "https://localhost:7023/images/products/centosalgado.png", "Cento de Salgados", 45.00m, null, "Cento" },
                    { 23, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo de 25 de cada sabor. Sabores: Misto, Queijo, Carne do sol, Frango, Pão pizza.", "https://localhost:7023/images/products/salgadoforno.png", "Cento de Salgados de Forno", 55.00m, null, "Cento" },
                    { 24, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sabores: Queijo, Queijo com orégano, Alho.", "https://localhost:7023/images/products/centoBiscoitinhoFornoFesta.png", "Cento de Biscoitinho de Forno", 50.00m, null, "Cento" },
                    { 25, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite, carne artesanal, alface, tomate, queijo, maionese e ketchup.", "https://localhost:7023/images/products/miniHamburguer.png", "Mini Hambúrguer (10 unidades)", 35.00m, null, "Pacote" },
                    { 26, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pão de leite, molho de tomate, batata palha, maionese e ketchup.", "https://localhost:7023/images/products/miniCachorroQuente.png", "Mini Cachorro Quente (10 unidades)", 20.00m, null, "Pacote" },
                    { 27, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mínimo 10 de cada sabor. Sabores: Salsicha, Queijo, Misto, Mistão, Frango, Carne do sol, Pizza, Calabresa.", "https://localhost:7023/images/products/miniSalgadoForno.png", "Mini Salgados de Forno (10 unidades)", 12.50m, null, "Pacote" },
                    { 28, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cheesecake com geleia de morango. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/cheesecake.png", "Cheesecake de Morango", 95.00m, null, "Unidade" },
                    { 29, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Guirlanda de brownie com brigadeiro e morangos. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/guirlandaBrownie.png", "Guirlanda de Brownie", 75.00m, null, "Unidade" },
                    { 30, true, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Fundo de biscoito, bananas, doce de leite e uma camada de chantily canela e chocolate. Serve de 15 a 20 pessoas.", "https://localhost:7023/images/products/banoffee.png", "Banoffee", 80.00m, null, "Unidade" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Bolo personalizado, serve 20 fatias.", "/img/bolo1.jpg", "Bolo Temático 1 Andar", 150.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "PromotionPrice", "UnitOfMeasure" },
                values: new object[] { false, "Brigadeiro com chocolate belga.", "/img/brigadeiro.jpg", "Cento de Brigadeiro Gourmet", 120.00m, 110.00m, "Cento" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { false, "Sabor do dia, consulte.", "/img/bolo_pote.jpg", "Bolo de Pote (Pronta Entrega)", 12.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Coxinhas crocantes e sequinhas.", "/img/coxinha.jpg", "Cento de Coxinha (Frango)", 90.00m, "Cento" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Name", "UnitOfMeasure" },
                values: new object[] { "Massa podre com recheio cremoso.", "/img/torta_frango.jpg", "Torta Salgada de Frango (Kg)", "Kg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { true, "Kit com 1 mini-bolo, 50 salgados, 20 doces.", "/img/kit_festa.jpg", "Kit Festa (10 pessoas)", 180.00m, "Kit" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { false, "Empadão cremoso de palmito.", "/img/empadao.jpg", "Empadão de Palmito (Kg)", 80.00m, "Kg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { "Bordado ponto cruz personalizado com nome.", "/img/toalha.jpg", "Toalha de Mesa Bordada", 250.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { false, "Vela artesanal, 200g.", "/img/vela.jpg", "Vela Aromática (Lavanda)", 45.00m, "Unidade" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Polvo de crochê, cores a escolher.", "/img/polvo.jpg", "Amigurumi (Polvo)", 90.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { false, "Cesta completa com pães, frutas e frios.", "/img/cesta_cafe.jpg", "Cesta de Café da Manhã Standard", 180.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { false, "Seleção de queijos e um vinho tinto.", "/img/cesta_vinho.jpg", "Cesta de Queijos e Vinhos", 260.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { true, "Taxa de montagem. Adicione os itens avulsos no pedido.", "/img/cesta_vazia.jpg", "Monte sua Cesta (Serviço)", 50.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Seleção de chocolates finos.", "/img/cesta_choco.jpg", "Cesta de Chocolates", 150.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Buquê com 6 rosas.", "/img/buque.jpg", "Buquê de Rosas (Pequeno)", 70.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { true, "Topo de bolo em papel lamicote. Informe nome e idade.", "/img/topo_bolo.jpg", "Topo de Bolo Personalizado", 35.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { true, "Convite em papel texturizado, com laço.", "/img/convite.jpg", "Convite de Casamento (Unidade)", 8.50m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "Cartela de adesivos tema 'Planner'.", "/img/adesivos.jpg", "Kit de Adesivos (Pronta Entrega)", 15.00m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Customizable", "Description", "ImageUrl", "Name", "Price", "UnitOfMeasure" },
                values: new object[] { true, "Caderno A5 com capa dura personalizada (envie a arte).", "/img/caderno.jpg", "Caderno Personalizado (Capa)", 55.00m, "Unidade" });
        }
    }
}
