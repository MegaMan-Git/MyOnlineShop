using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITokenEncoder
    {
        string Encode(string token);
        string Decode(string token);
    }
}
