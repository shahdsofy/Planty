
using System.ComponentModel.DataAnnotations.Schema;
using Planty.Enums;
using Planty.Models;

public class Order
{
	public int OrderID { get; set; }
	public DateTime OrderDate { get; set; }

	[NotMapped]
	public decimal TotalPrice { get; set; }

	// Forign Key
	public int UserID { get; set; }


	public string? ShippingAddress { get; set; }
	public string? Notes { get; set; }
	public OrderStatus Status { get; set; } = OrderStatus.Pending;

	// Navigation Properties
	public ICollection<OrderItem> OrderItems { get; set; }
}
