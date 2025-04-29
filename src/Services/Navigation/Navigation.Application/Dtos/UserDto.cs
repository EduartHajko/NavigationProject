using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Dtos
{
    public record UserDto(
     Guid Id,
     string Email,
     string DisplayName,
     string Status
 );
}
