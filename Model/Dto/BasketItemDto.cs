namespace EFarm.Server.Model.Dto
{
	public class BasketItemDto
	{
        public int Id { get; set; }
        public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
        public ProductDto? Product { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
    }
}
