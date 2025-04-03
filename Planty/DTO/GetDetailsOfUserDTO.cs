namespace Blog_Platform.DTO
{
    public class GetDetailsOfUserDTO:GetShortDetailsOfUserDTO
    {
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
