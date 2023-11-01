using Core;

namespace BLL.Services.Users;

public interface IUserReader<TUser> where TUser : ApplicationUser
{
    IQueryable<TUser> GetPagedAsNoTracking(int pageNumber = 0, int pageCount = int.MaxValue);
}