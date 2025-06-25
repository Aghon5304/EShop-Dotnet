using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ViewModels;
using ShoppingCart.Infrastructure.Producer;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.Services;

public class CartService : ICartAdder, ICartReader, ICartRemover, ICartProcess
{
    private readonly ICartRepository _repository;
    protected Queue<int> _CartProcesedIdsQueue;
    protected IKafkaProducer _kafkaProducer;
    public CartService(ICartRepository repository, IKafkaProducer kafkaProducer)
    {
        _repository = repository;
        _kafkaProducer = kafkaProducer;
        _CartProcesedIdsQueue = new Queue<int>();
    }
    public void AddProductToCart(int cartId, Product product)
    {
        var cart = _repository.FindById(cartId) ?? new Cart { Id = cartId };
        cart.Products.Add(product);
        _repository.Add(cart);
    }
    public void RemoveProductFromCart(int cartId, int productId)
    {
        var cart = _repository.FindById(cartId);
        if (cart != null)
        {
            var product = cart.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                cart.Products.Remove(product);
                _repository.Update(cart);
            }
        }
    }

    public Cart GetCart(int cartId)
    {
        var cart = _repository.FindById(cartId);
        if (cart == null) return null;

        return new Cart
        {
            Id = cart.Id,
            Products = cart.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList()
        };
    }

    public List<Cart> GetAllCarts()
    {
        return _repository.GetAll().Select(c => new Cart
        {
            Id = c.Id,
            Products = c.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList()
        }).ToList();
    }
    public string ProcessCart(int cartId, string email)
    {
        var cart = _repository.FindById(cartId);
        if (cart == null || cart.Products is null) throw new Exception("Cart not found");
        // here should be a check if payment is successful
        var List_of_products = "List of products bought:";
        decimal cost = 0;
        for (int i = 0; i < cart.Products.Count; i++)
        {
            Product x = cart.Products[i];
            List_of_products += $"\n{x.Name} Price: {x.Price}";
            cost += x.Price;
        }
        List_of_products += $"\nTotal cost: {cost}";
        ShoppingCartSendKafkaDTO message = new ShoppingCartSendKafkaDTO { Email = email, Message = List_of_products };
        _CartProcesedIdsQueue.Enqueue(123);
        _kafkaProducer.SendMessageAsync("after-cart-process-topic", message);
        cart.Products.Clear();
        _repository.Update(cart);
        return List_of_products;
    }
}
