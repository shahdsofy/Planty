using System.ComponentModel.DataAnnotations.Schema;
using Planty.Models;

public class Cart
{
	public int CartID { get; set; }
	public int NumberOfItems { get; set; }
	///[ForeignKey("User")]
	public int UserID { get; set; }
	//public User User { get; set; }
	public ICollection<CartItem> CartItems { get; set; }

}
