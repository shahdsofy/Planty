using Planty.Models.Enums;

namespace Planty.DTO
{
	public class CheckoutDTO
	{
		public string? ShippingAddress { get; set; }
		public string? Notes { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
	}
}
