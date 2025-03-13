using System.ComponentModel.DataAnnotations.Schema;
using Planty.Models;

public class Order
{
	public int OrderID { get; set; }
	public DateTime OrderDate { get; set; }
	[NotMapped]
	public decimal TotalAmount { get; set; }

	//Forign Key
	//[ForeignKey("User")]
	public int UserID { get; set; }

	//Navigation Property
	//public ICollection<User> Users { get; set; }
	//public ICollection<UsersOrders> UsersOrders { get; set; }
	//public User User { get; set; }
	public ICollection<OrderItem> OrderItems { get; set; }
}
