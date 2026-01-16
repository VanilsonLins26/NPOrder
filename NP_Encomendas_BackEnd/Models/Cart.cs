using System.Reflection.PortableExecutable;

namespace NP_Encomendas_BackEnd.Models;

public class Cart
{
    public CartHeader CartHeader { get; set; } = new CartHeader();
    public IEnumerable<CartItem> CartItems { get; set; } = Enumerable.Empty<CartItem>();
}