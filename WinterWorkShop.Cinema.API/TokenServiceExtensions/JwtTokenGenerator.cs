﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.TokenServiceExtensions
{
    public static class JwtTokenGenerator
    {
        // WARNING: This is just for demo purpose

        public static string Generate(string name, string role,Guid userId, string issuer, string key)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString())
            };
            if (role =="admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            if (role == "user")
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
            }
            if (role == "superUser")
            {
                claims.Add(new Claim(ClaimTypes.Role, "superUser"));
            }
            if (role == "guest")
            {
                claims.Add(new Claim(ClaimTypes.Role, "guest"));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
