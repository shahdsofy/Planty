public class Plant
{
	public int ID { get; set; }
	public string Name { get; set; }
	public string Type { get; set; }
	public decimal Price { get; set; }
	//Image file
	public string ImagePath { get; set; }

	public string Details { get; set; }
	// if no need order
	//public int? OrderID { get; set; }
	//public Order? Order { get; set; }
}
