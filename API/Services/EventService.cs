using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public interface IEventService
    {
        Task AddEventJoinUsers(AddEventJoinUserVM usersToEvent);
        Task CreateEvent(EventVM model);
        Task<EventVM?> GetEventById(Guid id);
        Task<IEnumerable<EventVM?>> GetEvents();
        Task UpdateEvent(EventVM model);
    }

    public class EventService : IEventService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly VueHwDbContext _db;

        public EventService(UserManager<AppUser> userManager, VueHwDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IEnumerable<EventVM?>> GetEvents()
        {
            var result = _db.Events
            .GroupJoin(_db.Eventjoinusers, e => e.Id, j => j.EventId,
             (e, j) => new EventVM()
             {
                 Id = e.Id,
                 Name = e.Name,
                 StartTime = e.StartTime.ToString("yyyy-MM-dd"),
                 EndTime = e.EndTime.ToString("yyyy-MM-dd"),
                 Address = e.Address,
                 Cost = e.Cost,
                 EventjoinusersName = j
                 .Select(z => new EventjoinusersVM { UserId = z.UserId, UserName = z.UserName }).ToList(),
             });

            return result;
        }

        public async Task<EventVM?> GetEventById(Guid id)
        {
            return _db.Events.Where(x => x.Id == id)
            .Select(e => new EventVM()
            {
                Id = e.Id,
                Name = e.Name,
                StartTime = e.StartTime.ToString("yyyy-MM-dd"),
                EndTime = e.EndTime.ToString("yyyy-MM-dd"),
                Address = e.Address,
                Cost = e.Cost,
                EventjoinusersName = _db.Eventjoinusers
                    .Where(x => x.EventId == e.Id)
                    .Select(y => new EventjoinusersVM()
                    {
                        UserId = y.UserId,
                        UserName = y.UserName,
                    }).ToList()
            }).FirstOrDefault();
        }

        public async Task UpdateEvent(EventVM model)
        {
            Event evt = await _db.Events.FindAsync(model.Id);

            if (evt == null)
            {
                return;
            }

            var allUsers = _db.Eventjoinusers.Where(x => x.EventId == model.Id).ToList();

            if (!(allUsers.Equals(model.EventjoinusersName)))
            {
                _db.Eventjoinusers.RemoveRange(allUsers);
            }

            evt.Id = model.Id;
            evt.Name = model.Name;
            evt.StartTime = DateTime.Parse(model.StartTime);
            evt.EndTime = DateTime.Parse(model.EndTime);
            evt.Address = model.Address;
            evt.Cost = model.Cost;

            if (model.EventjoinusersName.Count > 0)
            {
                await _db.Eventjoinusers.AddRangeAsync(model.EventjoinusersName.Select(x => new Eventjoinuser {
                    EventId = model.Id,
                    UserId = x.UserId,
                    UserName = x.UserName,
                }));
            }

            _db.Events.Update(evt);
            await this._db.SaveChangesAsync();
        }

        public async Task CreateEvent(EventVM model)
        {
            Event evt = new Event();
            var guid = Guid.NewGuid();

            evt.Id = guid;
            evt.Name = model.Name;
            evt.StartTime = DateTime.Parse(model.StartTime);
            evt.EndTime = DateTime.Parse(model.EndTime);
            evt.Address = model.Address;
            evt.Cost = model.Cost;
            
            if (model.EventjoinusersName?.Count > 0)
            {
                await _db.Eventjoinusers.AddRangeAsync(model.EventjoinusersName.Select(x => new Eventjoinuser {
                    EventId = guid,
                    UserId = x.UserId,
                    UserName = x.UserName,
                }));
            }

            await _db.Events.AddAsync(evt);
            await this._db.SaveChangesAsync();
        }


        public async Task AddEventJoinUsers(AddEventJoinUserVM usersToEvent)
        {
            var users = usersToEvent.JoinUsers.Select(x => new Eventjoinuser
            {
                EventId = usersToEvent.EventId,
                UserId = x.UserId,
                UserName = x.UserName
            });


            await _db.Eventjoinusers.AddRangeAsync(users);
            await this._db.SaveChangesAsync();
        }

    }
}