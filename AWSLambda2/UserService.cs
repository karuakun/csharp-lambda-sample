using System.Linq;
using AWSLambda2.Entities;

namespace AWSLambda2
{
    public class UserService
    {
        private readonly MyDbContext _dbContext;

        public UserService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string FindUser(string userName)
        {
            return _dbContext.Users
                .Where(u => u.Name.Contains(userName))
                .Select(u => u.Name)
                .FirstOrDefault();
        }
    }
}