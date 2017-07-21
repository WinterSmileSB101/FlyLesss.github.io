using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NUnit.Framework;
using System.Reflection;


namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    [TestFixture]
    public class DataAccessor : IDataAccessor
    {
        public DataAccessor()
        {
            // TODO: Implement this constructor
        }

        public DataAccessor(string connectionString)
        {
            // TODO: Implement this constructor
        }

        #region 查询操作  返回list集合
        public List<TEntity> Query<TEntity>(string condition) where TEntity : class, new()
        {
            //获取实体类型
            var type = typeof(TEntity);
            //获取实体公共属性  var自动识别为数组类型
            var pros = type.GetProperties();

            Console.WriteLine();
            Console.WriteLine("Query data from database ...");
            //创建List集合
            List<TEntity> lists = new List<TEntity>();

            using (var conn = DataAccess.SqlHelper.Instance.GetConnection())
            {
                //通过sqlHelper对数据库执行查询操作
                var reader = DataAccess.SqlHelper.Instance.ExecuteQuery(conn,
                   condition, null);
                //定义受影响行数
                var rowCount = 0;

                while (reader.Read())
                {
                    //定义一个实体类  方便每次赋值
                    TEntity entity = new TEntity();

                    foreach (var pro in pros)
                    {
                        //不清楚为什么会出现一个TypeId属性   
                        if (pro.Name.Equals("TypeId")) break;

                        pro.SetValue(entity, reader[pro.Name], null);
                        Console.WriteLine("\attribute.name:{0}\tpro.name:{1}", pro.Name, pro.GetValue(entity, null));

                    }
                    lists.Add(entity);
                    rowCount++;
                }

                Console.WriteLine("Total {0} row(s) query.", rowCount);
            }
            return lists;


        }
        #endregion

        #region  插入操作 返回受影响行数
        public int Create<TEntity>(TEntity entity) where TEntity : class
        {
            var type = typeof(TEntity);
            var pros = type.GetProperties();

            Console.WriteLine();
            Console.WriteLine("Inserting data into database ...");
            //受影响行数  >0 插入成功
            int insertedId;

            using (var conn = DataAccess.SqlHelper.Instance.GetConnection())
            {
                insertedId = DataAccess.SqlHelper.Instance.ExecuteScalar<int>(conn,
                    "INSERT INTO dbo.Amber(Name, InDate, InUser,LastEditDate,LastEditUser) VALUES(@Name, GETDATE(), 'Demo', GETDATE(),'Demo') SELECT SCOPE_IDENTITY()",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@Name", SqlDbType.NVarChar, 50) {Value = type.GetProperty("Name").GetValue(entity,null)}
                    });
                //插入ID == 受影响行数   因为数据库中设置为自增  所以这段代码可有可无
                type.GetProperty("ID").SetValue(entity, insertedId, null);
                Console.WriteLine("New record has been inserted, id {0}.", insertedId);

            }

            return insertedId;

        }
        #endregion

        #region   更新操作 返回受影响行数
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {

            var type = typeof(TEntity);
            var pros = type.GetProperties();

            Console.WriteLine();
            Console.WriteLine("Updating data into database ...");

            using (var conn = DataAccess.SqlHelper.Instance.GetConnection())
            {
                var rowEffected = DataAccess.SqlHelper.Instance.ExecuteNonQuery(conn,
                    "UPDATE TOP(1) dbo.Amber SET Name=@Name,LastEditDate=GETDATE(),LastEditUser='DEMO' WHERE ID=@ID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@ID", SqlDbType.Int) {Value = type.GetProperty("ID").GetValue(entity,null)},
                        new SqlParameter("@Name", SqlDbType.NVarChar, 50) {Value = type.GetProperty("Name").GetValue(entity,null)}
                    });

                Console.WriteLine("{0} row effected", rowEffected);

                return rowEffected;
            }


        }
        #endregion

        #region 删除操作  返回受影响行数
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {

            var type = typeof(TEntity);
            var pros = type.GetProperties();
            Console.WriteLine();
            Console.WriteLine("Deleting data from database ...");
            using (var conn = DataAccess.SqlHelper.Instance.GetConnection())
            {
                var rowEffected = DataAccess.SqlHelper.Instance.ExecuteNonQuery(conn,
                    "DELETE TOP(1) dbo.Amber WHERE ID=@ID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@ID", SqlDbType.Int) {Value =type.GetProperty("ID").GetValue(entity,null) }
                    });

                Console.WriteLine("{0} row effected", rowEffected);
                return rowEffected;
            }

        }
         #endregion 

        
    }

}
