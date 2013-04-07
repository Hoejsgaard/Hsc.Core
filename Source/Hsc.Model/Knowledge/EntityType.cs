namespace Hsc.Model.Knowledge
{
    public class EntityType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AttributeTypeCollection Attributes {get; private set;}

        public void AddAttributes(AttributeTypeCollection collection)
        {
            foreach (AttributeType attributeType in collection)
            {
                Attributes.Add(attributeType);
            }
        }

        public EntityType(string name) : this()
        {
            Name = name;
        }

        public EntityType()
        {
            Attributes = new AttributeTypeCollection();
        }

        private void AddDefaultAttributes()
        {
        }
    }
}
