using EFarm.Server.Model;

namespace EFarm.Server.Model
{
	public class BasketItem : BaseEntity
{

    public decimal UnitPrice { get;  set; }
    public int Quantity { get;  set; }
    public int ProductId { get;  set; }
    public int BasketId { get;  set; }
    public Product? Product { get;  set; }

        public BasketItem()
        {
            
        }

        public BasketItem(int productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}

}
