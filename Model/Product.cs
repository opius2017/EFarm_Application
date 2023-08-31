using EFarm.Server.Model;

namespace EFarm.Server.Model
{
	public class Product:BaseEntity
    {
        public string Name { get;  set; }
        public string? Description { get;  set; }
        public decimal? OldPrice { get;  set; }
        public decimal Price { get;  set; }
        public string? PictureUri { get;  set; }
        public Product()
        {
            
        }
        public Product(string name, decimal? oldPrice, decimal price)
		{
			Name = name;
			OldPrice = oldPrice;
			Price = price;
		}
		public Product(string name, string description, decimal price, string pictureUri)
		{
			Name = name;
			Description = description;
			Price = price;
			PictureUri = pictureUri;
		}
		public Product(string name, string description, decimal oldPrice, decimal price, string pictureUri)
		{
			Name = name;
			Description = description;
			Price = price;
			OldPrice = oldPrice;
			PictureUri = pictureUri;
		}
		public Product(string name, decimal oldPrice, decimal price, string pictureUri)
        {
            Name = name;
            OldPrice = oldPrice;
            Price = price;
            PictureUri = pictureUri;
        }

    }

}
