using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenEncoder : ITokenEncoder
    {
        public string Decode(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        public string Encode(string token)
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
    }
}
