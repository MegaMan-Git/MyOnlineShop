using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class Result
    {
        public bool IsSucceeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
