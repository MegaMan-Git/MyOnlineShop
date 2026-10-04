using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class Result
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsSucceeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
