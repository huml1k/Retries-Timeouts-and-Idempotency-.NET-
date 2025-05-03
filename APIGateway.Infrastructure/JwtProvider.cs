using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Infrastructure
{
    public class JwtProvider(IOptions<JwtOption> options)
    {
        private readonly JwtOption _option = options.Value;

        public string GenerateToken() 
        {

        }
    }
}
