using System;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;
using Hsc.SqlRepository;
using Microsoft.Practices.Unity;

namespace Hsc.TestApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUnityContainer conatiner = ContainerConfiguration.GetUnityContainer();

            var sqlRepository = conatiner.Resolve<ISqlRepository>();
            sqlRepository.InitializeDatabase();
            sqlRepository.PopulateWithMockTypes();
            sqlRepository.PopulateWithMockData();

            EntityType entityType = sqlRepository.EntityTypes.Get("Person");

            Console.WriteLine("Fetched Person EntityType from database");

            foreach (AttributeType attributeType in entityType.Attributes)
            {
                Console.WriteLine("   {0} : {1}", attributeType.Name, attributeType.DataType);
            }

            Console.WriteLine("The following Persons are in the database");
            foreach (Entity person in sqlRepository.Entities.GetAll(entityType))
            {
                Console.WriteLine("Person: ");
                PrintAttributes(person.Attributes, 1);
            }

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }

        private static void PrintAttributes(AttributeCollection attributes, int indentation)
        {
            foreach (Model.Operation.Attribute attribute in attributes)
            {
                if (attribute.AttributeType.DataType == DataType.Entity)
                {
                    Entity childEntity = (Entity) attribute.Value;
                    Console.WriteLine("{0}{1} : {2}", GetIndentation(indentation), attribute.Name, childEntity.Attributes["Name"].Value);
                }
                else
                {
                    Console.WriteLine("{0}{1} : {2}", GetIndentation(indentation), attribute.Name, attribute.Value);
                }
            }
        }

        private static string GetIndentation(int indents)
        {
            string indentation = String.Empty;
            if (indents <= 0)
            {
                return indentation;
            }
            for (int i = 0; i < indents; i++)
            {
                indentation += "    ";
            }
            return indentation;
        }
    }
}