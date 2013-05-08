namespace Hsc.Model.Knowledge
{
    public class EntityAttributeType : AttributeType
    {
        public EntityAttributeType()
        {
            DataType = DataType.Entity;
        }

        public EntityAttributeType(string name, EntityType ofType) : base(name, DataType.Entity)
        {
            OfType = ofType;
        }

        public EntityType OfType { get; set; }
    }
}