namespace Planty.Models
{
	public class OrderItem
	{
		public int OrderItemID { get; set; }
		public int OrderID { get; set; }
		public Order Order { get; set; }
		public int PlantID { get; set; }
		public Plant Plant { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
