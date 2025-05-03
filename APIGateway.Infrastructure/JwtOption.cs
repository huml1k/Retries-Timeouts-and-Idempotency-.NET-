using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Infrastructure
{
    public class JwtOption
    {
        public string SercretKey { get; set; } = string.Empty;

        public int ExpiresHours { get; set; }
    }
}
