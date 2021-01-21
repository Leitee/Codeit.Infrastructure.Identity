using Pandora.Infrastructure.Identity.Model.Dtos;
using Pandora.NetStdLibrary.Base.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Infrastructure.Identity.Interfaces
{
    public interface IAuthService
    {
        //Task<AuthResult<Token>> Login(LoginDTO loginDto);
        //Task<AuthResult<Token>> SignUp(SignUpDTO signUpDto);
        //Task<AuthResult<Token>> SignOut();
        Task<AuthResult<Token>> ChangePassword(ChangePasswordDTO changePasswordDto);
        Task<AuthResult<string>> RequestPassword(RequestPasswordDTO requestPasswordDto);
        Task<AuthResult<Token>> RestorePassword(RestorePasswordDTO restorePasswordDto);
        Task<AuthResult<Token>> RefreshToken(RefreshTokenDTO refreshTokenDto);
        Task<Token> GenerateToken(string userId);
    }
}
