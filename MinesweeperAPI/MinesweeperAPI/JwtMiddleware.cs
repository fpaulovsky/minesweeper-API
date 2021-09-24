using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperAPI
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            var playerId = jwtUtils.ValidateToken(token);
            if (playerId != null)
            {
                context.Items["PlayerId"] = playerId.Value;
            }

            await _next(context);
        }
    }
}
