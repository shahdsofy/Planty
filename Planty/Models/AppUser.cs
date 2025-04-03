using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Azure;
using System;

namespace Blog_Platform.Models
{
    public class AppUser : IdentityUser
    {
        public List<BlogPost> posts { get; set; } = new List<BlogPost>();
        public List<Comment> Comments { get; set; } = new List<Comment>(); //: List of Comment he writes on Posts.
        public Token Token  { get; set; }
    }
}
