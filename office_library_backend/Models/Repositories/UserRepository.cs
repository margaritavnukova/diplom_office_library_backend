using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private Entities1 db = new Entities1();

        public List<UsersDto> Initialize()
        {
            var query = from AspNetUsers in db.AspNetUsers.ToList() select AspNetUsers;
            var users = query.ToList();

            List<UsersDto> usersDto = new List<UsersDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UsersDto(user));
            }

            return usersDto;
        }

        public AspNetUsers Add(AspNetUsers item)
        {
            if (item == null)
                throw new ArgumentNullException();
            db.AspNetUsers.Add(item);
            db.SaveChanges();
            return item;
        }

        public UsersDto Get(string id)
        {
            var users = Initialize();
            return users.Find(p => p.Id == id);
        }

        public IEnumerable<UsersDto> GetAll()
        {
            var users = Initialize();
            return users;
        }

        public void Remove(string id)
        {
            var itemToDelete = db.AspNetUsers.Where(b => b.Id == id).FirstOrDefault();

            db.AspNetUsers.Remove(itemToDelete);
            db.SaveChanges();
        }

        public bool Update(UsersDto item)
        {
            var users = Initialize();
            if (item == null)
                throw new ArgumentNullException();
            int index = users.FindIndex(p => p.Id == item.Id);
            if (index == -1)
                return false;

            var itemToDelete = db.AspNetUsers.Where(p => p.Id == item.Id).FirstOrDefault();
            itemToDelete.PhoneNumber = item.PhoneNumber;
            db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}