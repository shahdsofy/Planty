using System.ComponentModel.DataAnnotations.Schema;
using Planty.Models;

public class Profile //weak entity
{
	// Composite Key UserID & ProfileNumber 
	public int ProfileNumber { get; set; }
	public string UserID { get; set; }
	///public User User { get; set; }
	public string? Bio { get; set; }
	public string? ProfilePicture { get; set; }

}
