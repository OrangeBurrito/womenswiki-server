using WomensWiki.Common.Interfaces;
using WomensWiki.Domain;

namespace WomensWiki.Features.Users.Persistence;

public interface IUserRepository : IRepository {
    Task<User?> GetUserByUsername(string username);
}