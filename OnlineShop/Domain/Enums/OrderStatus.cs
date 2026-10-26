using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        AwaitingPayment,
        Paid,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
