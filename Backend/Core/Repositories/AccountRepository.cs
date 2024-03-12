﻿using AutoMapper;
using Backend.Data;
using Backend.Models;
using Backend.Utils.Request;
using Backend.Utils.Response;
using Microsoft.EntityFrameworkCore;
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
        var account = _context.Users
            .Include(x => x.Role)
            .SingleOrDefault(x => x.Email == model.Email);
        
        // validate
        if (account == null || !BCrypt.Net.BCrypt.Verify(model.Password, account.Password))
        {
            throw new ApplicationException("Email or password is incorrect");
        }
        
        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtRepository.GenerateToken(account);
        var refreshToken = _jwtRepository.GenerateRefreshToken(ipAddress);
        account.RefreshTokens.Add(refreshToken);
        
        // remove old refresh tokens from account
        removeOldRefreshTokens(account);
        
        // save changes to db
        _context.Update(account);
        _context.SaveChanges();
        
        // return data in authenticate response object
        var response = _mapper.Map<AuthenticateResponse>(account);
        response.JwtToken = jwtToken;
        response.RefreshToken = refreshToken.Token;
        
        // set the Role properti in the response object
        response.Role = account.Role?.Name;
        
        return response;
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
    
    private void removeOldRefreshTokens(User account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_applicationSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }
}