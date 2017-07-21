using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NUnit.Framework;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    class MainForm
    {
        static void Main(string[] args)
        {
            DataAccessor da = new DataAccessor();
            NewAmber namber = new NewAmber();
            Type t = typeof(NewAmber);

            //插入操作
            t.GetProperty("Name").SetValue(namber, "amber", null);
            da.Create<NewAmber>(namber);


            //查询操作
            string condition = "select * from dbo.Amber";
            List<NewAmber> ambers = da.Query<NewAmber>(condition);
            foreach (var amber in ambers)
            {
                Console.WriteLine(amber.InDate);
            }
            
            //删除操作
            t.GetProperty("ID").SetValue(namber, 2, null);
            da.Delete<NewAmber>(namber);


            //更新操作
            t.GetProperty("ID").SetValue(namber, 1, null);
            t.GetProperty("Name").SetValue(namber, "amber_li", null);
            da.Update<NewAmber>(namber);
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
