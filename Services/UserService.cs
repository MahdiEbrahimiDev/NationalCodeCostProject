public class UserService : IUserService
{ private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Register(registerDto Dto)
    {
        if(Dto.password!=Dto.confirmedpassword)
        return false;
           if  (userExist=_context.Any(u => u.Username == dto.Username))
            return false;
            var user=new User
            {Username = dto.Username };
             user.PasswordHash=_passwordHasher.HashPassword(user, dto.Password);
             _context.User.Add(user);
             _context.SaveChangesAsync();
                     return true;

    }

     public async Task<User> login(loginDto Dto)
    {
        var user=await _context.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null)
            return null;
            var result=_passwordHasher.VerifyHashedPassword(user, user.PasswordHash,Dto.password);
             result==PasswordVerificationResult.Success ? user : null;
    }
}