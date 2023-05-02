using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment4
{
    public class DeleteOperation<T> where T:new()
    {
        private readonly SqlConnection _connection = new SqlConnection("Server=DESKTOP-86V0L02;Database=assignment_4;Trusted_Connection=True;");

        private T _t;

        public DeleteOperation(T t)
        {
            _t = t;
        }
        public void DeleteObject(object obj, string foreign_key, string refId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            Type type = obj.GetType();

            string tableName = string.Empty;

            if (type.IsClass)
            {
                tableName = type.Name;
            }

            string FkColumn = $"{tableName.ToLower()}id";

            PropertyInfo[] properties = type.GetProperties();

            var id = properties.FirstOrDefault(x => x.Name.ToLower() == "id")?.GetValue(obj);

            if (id == null)
            {
                throw new Exception("Object Must Have An ID Property.");
            }

            string? fkId = id.ToString();

            if (foreign_key != null && refId != null)
            {
                dict.Add(foreign_key, refId);
            }

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null) continue;
                if (value is string || value.GetType().IsValueType)
                {
                    dict.Add(property.Name, value);
                }
                else if (value is IList list)
                {
                    foreach (var item in list)
                    {
                        DeleteObject(item, FkColumn, fkId);
                    }
                }
                else
                {
                    DeleteObject(value, FkColumn, fkId);
                }
            }



            string deleteQuery = $"DELETE FROM {tableName} WHERE id=@id";

            using (SqlCommand cmd = new SqlCommand(deleteQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (_connection.State != ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                }
            }
        }




        //public void DeleteObjectById(object id, string NestedTableName)
        //{
        //    int flag = 0;
        //    Type type;
        //    if (flag == 0)
        //    {
        //        type = _t.GetType();
        //        flag++;
        //    }

        //    string tableName = string.Empty;
        //    //if (type.IsClass)
        //    //{
        //    //   tableName = type.Name;
        //    }

        //    string foreignKey = $"{tableName}id";

        //    PropertyInfo[] properties = type.GetProperties();
        //    foreach(var property in properties)
        //    {
        //        var value = property.GetValue(_t);
        //        if (value == null) continue;
        //        else if (value is IList list)
        //        {
        //            foreach (var item in list)
        //            {
        //                DeleteObjectById(id,tableName);
        //            }
        //        }
        //        else
        //        {
        //            DeleteObjectById(id, tableName);
        //        }
        //    }

        //    string query = $"delete from {tableName} where {foreignKey} = @id";
        //    using (SqlCommand cmd = new SqlCommand(query, _connection))
        //    {
        //        cmd.Parameters.AddWithValue("@id", id);

        //        try
        //        {
        //            _connection.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        finally
        //        {
        //            if (_connection.State != ConnectionState.Closed)
        //            {
        //                _connection.Close();
        //            }
        //        }
        //    }
        //}

        





    }

}
