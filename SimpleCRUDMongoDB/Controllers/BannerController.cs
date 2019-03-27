using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDMongoDB.Models;
using SimpleCRUDMongoDB.Services;

namespace SimpleCRUDMongoDB.Controllers
{
    [Route("api/Banner")]
    public class BannersController : ControllerBase
    {
        private readonly BannerService _bannerService;

        public BannersController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet("")]
        public ActionResult<List<Banner>> Get()
        {
            return _bannerService.Get();
        }

        [HttpGet("{id}", Name = "GetBanner")]
        public ActionResult<Banner> Get(int id)
        {
            var bannerFound = _bannerService.Get(id);

            if (bannerFound == null)
            {
                return NotFound();
            }

            return bannerFound;
        }

        [HttpPost("")]
        public ActionResult<Banner> Create([FromBody]Banner banner)
        {
            _bannerService.Create(banner);

            return CreatedAtRoute("GetBanner", new { id = banner.Id}, banner);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Banner banner)
        {
            var bannerFound = _bannerService.Get(id);

            if (bannerFound == null)
            {
                return NotFound();
            }
            
            _bannerService.Update(id, banner);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bannerFound = _bannerService.Get(id);

            if (bannerFound == null)
            {
                return NotFound();
            }

            _bannerService.Remove(bannerFound.Id);

            return NoContent();
        }
    }
}