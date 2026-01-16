using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Models;
using System.Reflection.Emit;

namespace NP_Encomendas_BackEnd.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<NotificationQueue> NotificationQueues { get; set; }


    protected override void OnModelCreating(ModelBuilder mb)
    {

        var seedDate = new DateTime(2025, 11, 8);
        mb.Entity<Product>().HasData(

     new
     {
         Id = 1,
         Name = "Mini Pizza (10 unidades)",
         Price = 15.00m,
         Description = "Mínimo 10 de cada sabor. Sabores: Calabresa, Misto, Carne do sol, Frango c/queijo, Queijo.",
         ImageUrl = "https://localhost:7023/images/products/miniPizza.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 2,
         Name = "Mini Empada (25 unidades)",
         Price = 30.00m,
         Description = "Mínimo 25 de cada sabor. Sabores: Doce de leite, Limão, Brigadeiro, M&M, Frango, Carne do sol.",
         ImageUrl = "https://localhost:7023/images/products/miniEmpada.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 3,
         Name = "Mini Pastéis (25 unidades)",
         Price = 27.50m,
         Description = "Mínimo 25 de cada sabor. Sabores: Queijo, Carne moída, Frango, Calabresa c/queijo, Misto, Carne do sol, Chocolate, Nutella, Queijo com goiaba, Banana com canela.",
         ImageUrl = "https://localhost:7023/images/products/miniPasteis.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },

     new
     {
         Id = 4,
         Name = "Travessa de Pavê",
         Price = 70.00m,
         Description = "Pavê de leite condensado, biscoito e coberto com uma ganache de chocolate. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/pave.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 5,
         Name = "Pavê de Abacaxi",
         Price = 75.00m,
         Description = "Mousse com pedacinhos de abacaxi e biscoito. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/paveAbacaxi.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 6,
         Name = "Pavê de Leite e Goiaba",
         Price = 65.00m,
         Description = "Creme de leite condensado e doce de goiaba. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/paveLeiteGoiaba.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },


     new
     {
         Id = 7,
         Name = "Mini Brownie (25 unidades)",
         Price = 30.00m,
         Description = "Sabores: Brigadeiro Colorido ou preto, Nutella, Tradicional.",
         ImageUrl = "https://localhost:7023/images/products/miniBrownie.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 8,
         Name = "Mini Bolo (20 unidades)",
         Price = 25.00m,
         Description = "Sabores: Brigadeiro, Cenoura com chocolate, Limão com doce de leite.",
         ImageUrl = "https://localhost:7023/images/products/miniBolo.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 9,
         Name = "Mini Biscoitos de Festa (100 unidades)",
         Price = 50.00m,
         Description = "Sabores: Goiabinha, Nutella, Gotas de chocolate, M&M.",
         ImageUrl = "https://localhost:7023/images/products/miniBiscoitosFesta.png",
         UnitOfMeasure = "Cento",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },

     new
     {
         Id = 10,
         Name = "Pão Americano",
         Price = 80.00m,
         Description = "Serve de 10 a 15 pessoas. Opções: Torta fria de frango ou Torta fria de frango c/goiabada.",
         ImageUrl = "https://localhost:7023/images/products/paoAmericano.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 11,
         Name = "Pão de Metro",
         Price = 45.00m,
         Description = "Serve de 10 a 15 pessoas. Sabores: Queijo e presunto, Frango e queijo, Carne do sol e queijo.",
         ImageUrl = "https://localhost:7023/images/products/paoMetro.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 12,
         Name = "Pão Trança",
         Price = 45.00m,
         Description = "Serve de 8 a 10 pessoas. Sabores: Calabresa e queijo, Presunto e queijo, Frango e requeijão, Carne do sol e queijo.",
         ImageUrl = "https://localhost:7023/images/products/paoTranca.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },


     new
     {
         Id = 13,
         Name = "Pudim de Leite",
         Price = 60.00m,
         Description = "Clássico pudim de leite. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/pudim.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 14,
         Name = "Torta de Limão",
         Price = 70.00m,
         Description = "Fundo de biscoito e um mousse de limão, coberto com chantily e raspas de limão. Serve 12 a 15 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/tortaLimao.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 15,
         Name = "Torta de Maracujá com Chocolate",
         Price = 80.00m,
         Description = "Fundo de biscoito de chocolate, mousse de maracujá e um delicioso mousse de chocolate. Serve 12 a 15 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/tortaMaracujaChocolate.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },


     new
     {
         Id = 16,
         Name = "Empadão de Frango",
         Price = 50.00m,
         Description = "Empadão de frango cremoso. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/empadaoFrango.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 17,
         Name = "Empadão de Carne do Sol",
         Price = 65.00m,
         Description = "Empadão de carne do sol e requeijão. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/empadaoCarneSol.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 18,
         Name = "Empadão de Camarão Cremoso",
         Price = 95.00m,
         Description = "Empadão de camarão cremoso e requeijão. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/empadaoCamarao.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },

     new
     {
         Id = 19,
         Name = "Mini Pão Árabe (25 unidades)",
         Price = 37.50m,
         Description = "Pão árabe, alface, queijo, presunto e requeijão.",
         ImageUrl = "https://localhost:7023/images/products/miniPaoArabe.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 20,
         Name = "Mini Galo Frio (10 unidades)",
         Price = 20.00m,
         Description = "Pão de leite e creme de frango.",
         ImageUrl = "https://localhost:7023/images/products/miniGaloFrio.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 21,
         Name = "Mini Croissant (10 unidades)",
         Price = 35.00m,
         Description = "Sabores: Queijo ou Misto.",
         ImageUrl = "https://localhost:7023/images/products/miniCroissant.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },


     new
     {
         Id = 22,
         Name = "Cento de Salgados",
         Price = 45.00m,
         Description = "Mínimo de 25 de cada sabor. Sabores: Coxinha, Bolinha, Carne, Misto, Carne do sol, Pastel de carne/queijo/misto.",
         ImageUrl = "https://localhost:7023/images/products/centosalgado.png",
         UnitOfMeasure = "Cento",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 23,
         Name = "Cento de Salgados de Forno",
         Price = 55.00m,
         Description = "Mínimo de 25 de cada sabor. Sabores: Misto, Queijo, Carne do sol, Frango, Pão pizza.",
         ImageUrl = "https://localhost:7023/images/products/salgadoforno.png",
         UnitOfMeasure = "Cento",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },
     new
     {
         Id = 24,
         Name = "Cento de Biscoitinho de Forno",
         Price = 50.00m,
         Description = "Sabores: Queijo, Queijo com orégano, Alho.",
         ImageUrl = "https://localhost:7023/images/products/centoBiscoitinhoFornoFesta.png",
         UnitOfMeasure = "Cento",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },

     new
     {
         Id = 25,
         Name = "Mini Hambúrguer (10 unidades)",
         Price = 35.00m,
         Description = "Pão de leite, carne artesanal, alface, tomate, queijo, maionese e ketchup.",
         ImageUrl = "https://localhost:7023/images/products/miniHamburguer.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 26,
         Name = "Mini Cachorro Quente (10 unidades)",
         Price = 20.00m,
         Description = "Pão de leite, molho de tomate, batata palha, maionese e ketchup.",
         ImageUrl = "https://localhost:7023/images/products/miniCachorroQuente.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 27,
         Name = "Mini Salgados de Forno (10 unidades)",
         Price = 12.50m,
         Description = "Mínimo 10 de cada sabor. Sabores: Salsicha, Queijo, Misto, Mistão, Frango, Carne do sol, Pizza, Calabresa.",
         ImageUrl = "https://localhost:7023/images/products/miniSalgadoForno.png",
         UnitOfMeasure = "Pacote",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = true
     },

     new
     {
         Id = 28,
         Name = "Cheesecake de Morango",
         Price = 95.00m,
         Description = "Cheesecake com geleia de morango. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/cheesecake.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 29,
         Name = "Guirlanda de Brownie",
         Price = 75.00m,
         Description = "Guirlanda de brownie com brigadeiro e morangos. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/guirlandaBrownie.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     },
     new
     {
         Id = 30,
         Name = "Banoffee",
         Price = 80.00m,
         Description = "Fundo de biscoito, bananas, doce de leite e uma camada de chantily canela e chocolate. Serve de 15 a 20 pessoas.",
         ImageUrl = "https://localhost:7023/images/products/banoffee.png",
         UnitOfMeasure = "Unidade",
         CreateTime = seedDate,
         PromotionPrice = (decimal?)null,
         Active = true,
         Customizable = false
     }
 );
    }
}
