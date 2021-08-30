using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppApi.Data;
using DatingAppApi.Dtos;
using DatingAppApi.Helpers;
using DatingAppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatingAppApi.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    //[Route("api/[controller]")]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IDataRepository _Repo;
        private readonly IMapper _Mapper;
        private readonly IOptions<CloudinarySettings> _CloudinaryConfig;
        private readonly Cloudinary _Cloudinary;

        public PhotosController(IDataRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _Repo = repo;
            _Mapper = mapper;
            _CloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _CloudinaryConfig.Value.CloudName,
                _CloudinaryConfig.Value.ApiKey,
                _CloudinaryConfig.Value.ApiSecret
                );

            _Cloudinary = new Cloudinary(acc);
        }

        // GET: api/<PhotosController1>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<PhotosController1>/5
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _Repo.GetPhoto(id);

            var photo = _Mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoDto)
        {
            var user = await _Repo.GetUser(userId);

            if (user == null)
                return BadRequest("Could not find user");

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUserId != user.Id)
                return Unauthorized();

            var file = photoDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _Cloudinary.Upload(uploadParams);
                }
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _Mapper.Map<Photo>(photoDto);

            photo.User = user;

            if (!user.Photos.Any(m => m.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);



            if (await _Repo.SaveAll())
            {
                var photoToReturn = _Mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var photoFromRepo = await _Repo.GetPhoto(id);
            if (photoFromRepo == null)
                return NotFound();

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _Repo.GetMainPhotoForUser(userId);
            if (currentMainPhoto != null)
                currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _Repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var photoFromRepo = await _Repo.GetPhoto(id);
            if (photoFromRepo == null)
                return NotFound();

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete the main photo");

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _Cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                    _Repo.Delete(photoFromRepo);
            }

            if (photoFromRepo.PublicId == null)
            {
                _Repo.Delete(photoFromRepo);
            }

            if (await _Repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }


        //// POST api/<PhotosController1>
        //[HttpPost]
        //public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoDto)
        //{
        //    var user = await _Repo.GetUser(userId);
        //    var currentUserId = 0;

        //    if (user != null)
        //    {
        //        // *************** ERROR **************************8
        //        if ((User.FindFirst(ClaimTypes.NameIdentifier).Value).Length.ToString() != null)
        //        {
        //            currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //            if (currentUserId != user.Id)
        //            {
        //                return Unauthorized();
        //            }

        //            var file = photoDto.File;

        //            var uploadResult = new ImageUploadResult();

        //            if (file.Length > 0)
        //            {
        //                using (var stream = file.OpenReadStream())
        //                {
        //                    var uploadParams = new ImageUploadParams()
        //                    {
        //                        File = new FileDescription(file.Name, stream)
        //                    };

        //                    uploadResult = _Cloudinary.Upload(uploadParams);
        //                }
        //            }

        //            photoDto.Url = uploadResult.Uri.ToString();
        //            photoDto.PublicId = uploadResult.PublicId;

        //            var photo = _Mapper.Map<Photo>(photoDto);
        //            photo.User = user;

        //            if (!user.Photos.Any(m => m.IsMain))
        //            {
        //                photo.IsMain = true;
        //            }

        //            user.Photos.Add(photo);

        //            var photoToReturn = _Mapper.Map<PhotoForReturnDto>(photo);
        //            if (await _Repo.SaveAll())
        //            {
        //                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
        //            }

        //            return BadRequest("Could not add the photo.");
        //        }
        //        else
        //        {
        //            return BadRequest("User is Unauthorized...");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Could not find User");
        //    }
        //}

        // PUT api/<PhotosController1>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<PhotosController1>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
