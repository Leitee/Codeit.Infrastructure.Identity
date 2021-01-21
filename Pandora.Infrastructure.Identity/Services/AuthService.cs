using Pandora.Infrastructure.Identity.Interfaces;
using Pandora.Infrastructure.Identity.Model.Dtos;
using Pandora.Infrastructure.Identity.Model.Entities;
using Pandora.NetStdLibrary.Base.Security;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Pandora.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        protected readonly UserManager<IdentityAppUser> _userManager;

        public AuthService(UserManager<IdentityAppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResult<string>> RequestPassword(RequestPasswordDTO requestPasswordDto)
        {
            if (requestPasswordDto == null ||
                string.IsNullOrEmpty(requestPasswordDto.Email))
                return AuthResult<string>.UnvalidatedResult;

            var user = await _userManager.FindByEmailAsync(requestPasswordDto.Email);

            if (user != null && !string.IsNullOrEmpty(user.Id) && !user.Deleted)
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                return AuthResult<string>.TokenResult(passwordResetToken);
            }

            return AuthResult<string>.UnvalidatedResult;
        }

        public async Task<AuthResult<Token>> RestorePassword(RestorePasswordDTO restorePasswordDto)
        {
            if (restorePasswordDto == null ||
                string.IsNullOrEmpty(restorePasswordDto.Email) ||
                string.IsNullOrEmpty(restorePasswordDto.Token) ||
                string.IsNullOrEmpty(restorePasswordDto.NewPassword) ||
                string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
                string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
                restorePasswordDto.ConfirmPassword != restorePasswordDto.NewPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            var user = await _userManager.FindByEmailAsync(restorePasswordDto.Email);

            if (user != null && !string.IsNullOrEmpty(user.Id) && !user.Deleted)
            {
                var result = await _userManager.ResetPasswordAsync(user, restorePasswordDto.Token, restorePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    var token = new Token(); // JwtManager.GenerateToken(await _userManager.claim CreateIdentityAsync( (user,  ));
                    return AuthResult<Token>.TokenResult(token);
                }
            }

            return AuthResult<Token>.UnvalidatedResult;
        }

        public async Task<AuthResult<Token>> ChangePassword(ChangePasswordDTO changePasswordDto)
        {
            if (changePasswordDto == null ||
                string.IsNullOrEmpty(changePasswordDto.ConfirmPassword) ||
                string.IsNullOrEmpty(changePasswordDto.Password) ||
                changePasswordDto.Password != changePasswordDto.ConfirmPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            if (!string.IsNullOrEmpty(changePasswordDto.UserId))
            {
                var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
                var result = await _userManager.ChangePasswordAsync(user, null, changePasswordDto.Password);
                if (result.Succeeded)
                    return AuthResult<Token>.SucceededResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<AuthResult<Token>> RefreshToken(RefreshTokenDTO refreshTokenDto)
        {
            var refreshToken = refreshTokenDto?.Token?.Refresh_token;
            if (string.IsNullOrEmpty(refreshToken))
                return AuthResult<Token>.UnvalidatedResult;

            try
            {
                //var principal = JwtManager.GetPrincipal(refreshToken, isAccessToken: false);
                //int.TryParse(principal.Identity.GetUserId(), out var currentUserId);

                var user = new IdentityAppUser(); //await _userManager.FindByIdAsync(currentUserId); JwtSecurityToken

                if (user != null && !string.IsNullOrEmpty(user.Id) && !user.Deleted)
                {
                    var token = new Token();// JwtManager.GenerateToken(await _userManager.CreateIdentityAsync(user));
                    return await Task.FromResult(AuthResult<Token>.TokenResult(token));
                }
            }
            catch (Exception)
            {
                return AuthResult<Token>.UnauthorizedResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<Token> GenerateToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null && !string.IsNullOrEmpty(user.Id))
            {
                return new Token();// JwtManager.GenerateToken(await _userManager.CreateIdentityAsync(user));
            }

            return null;
        }
    }
}
