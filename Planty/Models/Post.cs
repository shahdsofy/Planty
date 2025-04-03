using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planty.Models
{
	public class Post
	{
		public int PostID { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		//[ForeignKey("User")]
		public int UserID { get; set; }
		//public User User { get; set; } //M:1
		public ICollection<Comment> Comments { get; set; } //1:M
	}
}
