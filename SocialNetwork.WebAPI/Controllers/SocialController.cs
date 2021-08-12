using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialNetwork.WebAPI.Controllers
{
    [Authorize]
    public class SocialController : ApiController
    {
        public IHttpActionResult Get()
        {
            SocialService noteService = CreateNoteService();
            var notes = noteService.GetNotes();
            return Ok(notes);
        }

        public IHttpActionResult Post(SocialCreate social)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var service = CreateNoteService();

            if (!service.CreateSocial(social)) 
                return InternalServerError();
            
            return Ok();
        }

        private SocialService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var socialService = new SocialService(userId);
            return socialService;
        }

        public IHttpActionResult Get(int id)
        {
            SocialService socialService = CreateNoteService();
            var social = socialService.GetSocialById(id);
            return Ok(social);
        }

        public IHttpActionResult Put(SocialEdit social)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateNoteService();

            if (!service.UpdateSocial(social))
                return InternalServerError();

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var service = CreateNoteService();
            if (!service.DeleteSocial(id))
                return InternalServerError();

            return Ok();
        }
    }
}
