using Core;
using DAL.Context;

namespace BLL.Services.Users;

public class ClientService
{
    private readonly ApplicationContext _context;
    
    public ClientService(ApplicationContext context)
    {
        _context = context;
    }

  
}