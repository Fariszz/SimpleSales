using AutoMapper;
using Backend.Data;
using Backend.Models;
using Backend.Utils.Request;
using Backend.Utils.Response;
using Microsoft.Extensions.Options;
using ApplicationException = Backend.Data.ApplicationException;

namespace Backend.Core.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtRepository _jwtRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IMapper _mapper;
    
    public AccountRepository(
        ApplicationDbContext context,
        IJwtRepository jwtRepository,
        IOptions<ApplicationSettings> appSettings,
        IMapper mapper)
    {
        _context = context;
        _jwtRepository = jwtRepository;
        _applicationSettings = appSettings.Value;
        _mapper = mapper;
    }
    
    public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
    {
        throw new System.NotImplementedException();
    }

    public void Register(RegisterRequest model, string origin)
    {
        // validate
        if (_context.Users.Any(x => x.Email == model.Email))
        {
            throw  new ApplicationException($"Email '{model.Email}' is already registered");
        }
        
        // map model to new user object
        var user = _mapper.Map<User>(model);
        
        // first registered user is an admin
        var isFirstUser = _context.Users.Count() == 0;
        user.RoleId = isFirstUser ? 1 : 2;
        user.Created = DateTime.UtcNow;
        
        // hash password
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        
        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}