using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDMongoDB.ExtensionMethods;
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

        [HttpGet("{id}", Name = "Get")]
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

            return CreatedAtRoute("Get", new { id = banner.Id}, banner);
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

        /// <summary>
        /// Returns Html string of the Banner model, check if string is valid Html
        /// </summary>
        /// <param name="id">Banner Id</param>
        /// <returns>Html</returns>
        [HttpGet("GetBanner/{id}")]
        public ActionResult<string> GetBanner(int id)
        {
            var bannerFound = _bannerService.Get(id);

            if (bannerFound == null)
            {
                return NotFound();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(bannerFound.Html);
            var htmlBanner = doc.ParseErrors.Any() ? bannerFound.Html.ToBannerHtml() : bannerFound.Html;
            return htmlBanner;
        }
    }
}