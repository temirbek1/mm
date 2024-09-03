using System;
using System.Runtime.Serialization;

namespace Resto.Front.Api.Dto.Abstract
{
    [DataContract]
    public abstract class EntityBase : IEntity
    {
        [DataMember(Name = "Id", IsRequired = true, Order = 0)]
        public Guid Id { get; set; }
    }
}
