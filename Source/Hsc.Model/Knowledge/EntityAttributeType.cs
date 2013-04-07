namespace Hsc.Model.Knowledge
{
    public class EntityAttributeType : AttributeType
    {
        public EntityType OfType { get; set; }

        public EntityAttributeType ()
        {
            DataType = DataType.Entity;
        }

        public EntityAttributeType(string name, EntityType ofType) : base(name, DataType.Entity)
        {
            OfType = ofType;
        }
    }
}
