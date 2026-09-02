using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Payment
{
    public class AdminChangeStatusDto
    {
        public int OrderId { get; set; }
        public PaymentStatus Status{ get; set; }
    }
}
