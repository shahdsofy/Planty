using System.ComponentModel.DataAnnotations.Schema;

namespace Planty.Models
{
	public class Stock  //weak entity
	{
		//Composite key AdminID &StockID
		public int StockID { get; set; }

		[ForeignKey("Plant")]
		public int PlantID { get; set; }
		public Plant Plant { get; set; }
		public int Quantity { get; set; }

	}
}
