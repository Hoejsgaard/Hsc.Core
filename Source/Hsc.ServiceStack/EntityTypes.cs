using System.Runtime.Serialization;

namespace Hsc.ServiceStack
{
    [DataContract]
    public class EntityTypes
    {
        [DataMember]
        public string Filter {get; set;}
    }
}