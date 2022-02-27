using OnlineAuctionApp.Core.DataAccess.Abstract;
using OnlineAuctionApp.Core.Entities;

namespace OnlineAuctionApp.Infrastructure.DataAccess.Concrete
{
    public class UserRepository : Repository<Employee>, IUserRepository
    {
        private readonly WebApplicationContext _context;

        public UserRepository(WebApplicationContext context) : base(context) => _context = context;
    }
}
