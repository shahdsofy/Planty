namespace Planty.Models
{
	public class CartItem
	{
		public int CartItemID { get; set; }
		public int CartID { get; set; }
		public Cart Cart { get; set; }// 1:1
		public int PlantID { get; set; }
		public Plant Plant { get; set; }//1:1
		public int Quantity { get; set; }
	}
}
