using System.Runtime.Serialization;
using Hsc.Model.Knowledge;

namespace Hsc.ServiceStack
{
    [DataContract]
    public class EntityTypeResponse
    {
        [DataMember]
        public EntityType EntityType { get; set; }
    }
}