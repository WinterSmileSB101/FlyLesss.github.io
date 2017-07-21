using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = true)]
    public class TableAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class ColumnAttribute : System.Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public string Name { get; private set; }
        public Type DataType { get; set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }

    [Table("dbo.Amber")]
    public class NewAmber :System.Attribute
    {
        [Column("ID", DataType = typeof(int), IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("InDate", DataType = typeof(DateTime))]
        public DateTime InDate { get; set; }
        [Column("InUser")]
        public string InUser { get; set; }
        [Column("LastEditDate", DataType = typeof(DateTime))]
        public DateTime LastEditDate { get; set; }
        [Column("LastEditUser")]
        public string LastEditUser { get; set; }
        
       

    }
    [TestFixture]
    public class AttributeTest
    {
       [Test]
        public void Test()
        {
           
            var type = typeof(NewAmber);

            var tableAttribute = (TableAttribute)type.GetCustomAttributes(
                typeof(TableAttribute),true
                )[0];
            Assert.AreEqual("dbo.Amber",tableAttribute.Name);  //true

            var properties = type.GetProperties();
           
            foreach (var propertyInfo in properties)
            {
                //propertyInfo.GetCustomAttributes（）获取属性的自定义特性
                var attribute =
                    (ColumnAttribute)propertyInfo.GetCustomAttributes(
                        typeof(ColumnAttribute), true
                        )[0];
                if(attribute != null)  
                Console.WriteLine(attribute.Name);
            }
        }

    }

   
}
