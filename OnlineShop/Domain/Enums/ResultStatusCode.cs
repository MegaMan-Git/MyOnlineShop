using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum ResultStatusCode
    {
        Success,
        BadRequest,
        Unauthorized,
        NotFound,
        Conflict,
        InternalServerError
    }
}
