using SocialNetwork.Data;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services
{
    public class SocialService
    {
        private readonly Guid _userId;

        public SocialService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateSocial(SocialCreate model)
        {
            var entity =
                new Social()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Socials.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<SocialListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Socials
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                                e =>
                                    new SocialListItem
                                    {
                                        SocialId = e.SocialId,
                                        Title = e.Title,
                                        CreatedUtc = e.CreatedUtc
                                    }
                                );
                return query.ToArray();
            }
        }

        public SocialDetail GetSocialById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Socials
                        .Single(e => e.SocialId == id && e.OwnerId == _userId);
                return
                    new SocialDetail
                    {
                        SocialId = entity.SocialId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool DeleteSocial(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSocial(SocialEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Socials
                        .Single(e => e.SocialId == model.SocialId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool SocialNote(int socialId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Socials
                        .Single(e => e.SocialId == socialId && e.OwnerId == _userId);
                ctx.Socials.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
