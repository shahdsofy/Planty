namespace Planty.Models
{
	public class Comment
	{
		public int CommentID { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public int PostID { get; set; }
		public Post Post { get; set; }
		public int UserID { get; set; }
		//public User User { get; set; }
	}
}
