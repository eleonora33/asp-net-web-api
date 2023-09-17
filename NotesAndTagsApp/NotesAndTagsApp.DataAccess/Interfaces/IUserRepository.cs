using NotesAndTagsApp.Domain.Models;

namespace NotesAndTagsApp.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
        User LoginUser(string  username, string hashedPassword);      
    }
}
