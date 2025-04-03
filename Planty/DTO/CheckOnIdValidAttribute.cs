using Blog_Platform.IRepository;
using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    internal class CheckOnIdValidAttribute<T> : ValidationAttribute
    where T : class,IModelHelper
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) 
                return null;
            IRepo<T>? Repo = null;
            if(typeof(T) == typeof(BlogPost))
                Repo = Repo = validationContext.GetService<IBlogPostRepo>() as IRepo<T>;
            else if(typeof(T) == typeof(Comment))
                Repo = Repo = validationContext.GetService<ICommentRepo>() as IRepo<T>;
            else if(typeof(T) == typeof(Tag))
                Repo = Repo = validationContext.GetService<ITagRepo>() as IRepo<T>;
            else if(typeof(T) == typeof(BlogPostHasTag))
                Repo = Repo = validationContext.GetService<IBlogPostHasTagRepo>() as IRepo<T>;
            
            
            if (Repo is null)
                return new ValidationResult("can't Provide need Service");
            if (Repo.CheckIdExist((int) value))
                return ValidationResult.Success;
            return new ValidationResult($"Id Of {nameof(T)} InValid");
        }
    }
}
