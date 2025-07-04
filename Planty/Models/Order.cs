﻿
using System.ComponentModel.DataAnnotations.Schema;
using Blog_Platform.Models;
using Planty.Models;
using Planty.Models.Enums;

public class Order
{
	public int OrderID { get; set; }
	public DateTime OrderDate { get; set; }

	[NotMapped]
	public decimal TotalPrice { get; set; }

	// Forign Key
	public string UserID { get; set; }


	public string? ShippingAddress { get; set; }
	public string? Notes { get; set; }
	public OrderStatus Status { get; set; } = OrderStatus.Pending;
	public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

	// Navigation Properties
	public ICollection<OrderItem> OrderItems { get; set; }

	public AppUser AppUser { get; set; }
}
