using EFarm.Server.Model;

namespace EFarm.Server.Model
{
	public class Basket : BaseEntity
    {
        public string BuyerId { get; set; }
         public List<BasketItem> Items { get; set; } = new();

        public int TotalItems => Items.Sum(i => i.Quantity);


        public Basket(string buyerId)
        {
            BuyerId = buyerId;
        }

        
    }

}
