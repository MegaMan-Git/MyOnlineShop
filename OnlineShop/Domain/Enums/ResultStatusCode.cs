using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum ResultStatusCode
    {
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500
    }
}
