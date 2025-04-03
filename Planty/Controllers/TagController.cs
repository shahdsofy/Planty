using Blog_Platform.DTO;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic;

namespace Blog_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagRepo tagRepo;

        public TagController(ITagRepo tagRepo)
        {
            this.tagRepo = tagRepo;
        }
        [HttpPost]
        public ActionResult<GeneralResponse> CreateTag(AddTagDTO addTag)//: Allow users to create new tags.
        {
            if (ModelState.IsValid) 
            {
                Tag tag = new Tag() 
                {
                    Name = addTag.Name,
                };
                tagRepo.Add(tag);
                tagRepo.Save();
                return new GeneralResponse()
                {
                    Success = true,
                    Content = "Add Tag Success"
                };
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage)
            };
        }
        [HttpGet("GetAll")]
        public ActionResult<GeneralResponse> GetAllTags()//: Retrieve a list of all tags.
        {
            List<ShowDataForTags> showShortDatas = new List<ShowDataForTags>();
            List<Tag> tags = tagRepo.GetAll();
            foreach (var item in tags)
            {
                showShortDatas.Add(new ShowDataForTags()
                {
                    Name = item.Name,
                    Id = item.Id,
                });
            }
            return new GeneralResponse() 
            {
                Success = true,
                Content = showShortDatas
            };
        }
        [HttpGet("{Id:int}")]
        public ActionResult<GeneralResponse> GetTagById(int Id)//: Retrieve details of a specific tag.
        {
            if(tagRepo.CheckIdExist(Id))
            {
                Tag tag = tagRepo.GetById(Id)!;
                ShowDataForTags showDatas = new ShowDataForTags() 
                {
                    Id = tag.Id,
                    Name = tag.Name
                };
                
                return new GeneralResponse()
                {
                    Success = true,
                    Content = showDatas
                };
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = "Invalid Tag Id"
            };
        }
        [HttpPut]
        public ActionResult<GeneralResponse> UpdateTag(UpdateTagDTO updateTag)//: Allow users to update tags.
        {
            if (ModelState.IsValid)
            {
                Tag tag = new Tag() 
                {
                    Id = updateTag.Id,
                    Name = updateTag.Name,
                }; 
                tagRepo.Update(tag);
                tagRepo.Save();
                return new GeneralResponse()
                {
                    Success = true,
                    Content = "Update Tag Success"
                };
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = "Invalid Tag Id"
            };
        }
        [HttpDelete]
        public ActionResult<GeneralResponse> DeleteTag(int Id)//: Allow users to delete tags.
        { 
            if (tagRepo.CheckIdExist(Id))
            {
                tagRepo.Delete(Id);
                tagRepo.Save();
                return new GeneralResponse() 
                {
                    Success = true,
                    Content = "Delete Tag Success"
                };
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = "Invalid Tag Id"
            };
        }
    }
}
