using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Domain.Entities;

namespace NetFilm.Domain.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<string> roles);
    }
}
