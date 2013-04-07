using System.Collections.Generic;
using System.Runtime.Serialization;
using Hsc.Model.Knowledge;

namespace Hsc.ServiceStack
{
    [DataContract]
    public class EntityTypesResponse
    {
        [DataMember]
        public List<EntityType> EntityTypes { get; set; }
    }
}