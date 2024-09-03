using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resto.Front.Api.Dto.Abstract
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
