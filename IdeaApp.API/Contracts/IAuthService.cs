using System.Threading.Tasks;
using IdeaApp.API.Models;

namespace IdeaApp.API.Contracts
{
         public interface IAuthService
    {
         Task<User> Register(User user, string password);

         Task<User> Login(string username,string password);

         Task<bool> UserExists(string username);
    }
    
}