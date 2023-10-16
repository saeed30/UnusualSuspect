using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Identity;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Common.Utilities;

namespace UnusualSuspect.Services.Identity;

public class JwtService : IJwtService
{
	private readonly ProjectSetting _siteSetting;
	private readonly SignInManager<ApplicationUser> signInManager;

	public JwtService(IOptionsSnapshot<ProjectSetting> settings, SignInManager<ApplicationUser> signInManager)
	{
		_siteSetting = settings.Value;
		this.signInManager = signInManager;
	}

	public async Task<AccessToken> GenerateAsync(ApplicationUser user)
	{
		var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);
		var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

		var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
		var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

		var claims = await _getClaimsAsync(user);

		var descriptor = new SecurityTokenDescriptor
		{
			Issuer = _siteSetting.JwtSettings.Issuer,
			Audience = _siteSetting.JwtSettings.Audience,
			IssuedAt = DateTime.Now,
			NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
			Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
			SigningCredentials = signingCredentials,
			EncryptingCredentials = encryptingCredentials,
			Subject = new ClaimsIdentity(claims)
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
		return new AccessToken()
		{
			access_token = new JwtSecurityTokenHandler().WriteToken(securityToken),
			expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds,
			token_type = "Bearer"
		};
	}

	private async Task<IEnumerable<Claim>> _getClaimsAsync(ApplicationUser user)
	{
		var result = await signInManager.ClaimsFactory.CreateAsync(user);
		var list = new List<Claim>(result.Claims);
		if (user.PhoneNumber != null)
			list.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
		return list;
	}
}
