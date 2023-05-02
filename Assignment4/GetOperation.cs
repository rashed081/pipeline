using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class GetOperation<G,T>
    {
        private readonly SqlConnection _connection = ConnectionString._connection;
        public List<Dictionary<string, object>> ExtractData(T obj, G id, string tableName)
        {
            var result = new List<Dictionary<string, object>>();

            StringBuilder query = new StringBuilder();

            query.Append($"select * from {obj.GetType().Name} where {tableName}id = @id");
            //select * from course where id = 1
            // select * from instructor where courseid = 1

            var data = ExecuteQuery(query.ToString(), id);
            result.AddRange(data);

            // result {}


            Type type = obj.GetType();

            PropertyInfo[] properties = type.GetProperties();

            if (type.IsClass)
            {
                if (type.IsGenericType)
                {
                    tableName = type.Name;
                    tableName = tableName.Remove(tableName.Length - 2, 2);
                }
                else
                    tableName = type.Name;
            }

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(obj);

                if (value == null) continue;

                else if (value is string || value.GetType().IsValueType) continue;

                else if (value is IList list)
                {
                    foreach (var item in list)
                    {
                        var subData = ExtractData((T)item, id, tableName);
                        result.AddRange(subData);
                    }
                }
                else
                {
                    var subData = ExtractData((T)value, id, tableName);
                    result.AddRange(subData);
                }
            }

            return result;
        }



        //public List<Dictionary<string, object>> ExtractAllData<T>(object obj) where T : new()
        //{
        //    var result = new List<Dictionary<string, object>>();

        //    Type type = typeof(T);

        //    string tableName = string.Empty;
        //    if (type.IsClass)
        //    {
        //        if (type.IsGenericType)
        //        {
        //            tableName = type.Name;
        //            tableName = tableName.Remove(tableName.Length - 2, 2);
        //        }
        //        else
        //            tableName = type.Name;
        //    }

        //    StringBuilder query = new StringBuilder();

        //    query.Append($"select * from {tableName}");

        //    var data = ExecuteQuery(query.ToString(), null);

        //    result.AddRange(data);

        //    Console.WriteLine(tableName);

        //    PropertyInfo[] properties = type.GetProperties();

        //    foreach (PropertyInfo property in properties)
        //    {
        //        var value = property.GetValue(obj);

        //        if (value == null) continue;

        //        else if (value is string || value.GetType().IsValueType) continue;

        //        else if (value is IList list)
        //        {
        //            foreach (var item in list)
        //            {
        //                var subData = ExtractAllData<T>(item);
        //                result.AddRange(subData);
        //            }
        //        }
        //        else 
        //        {
        //            var subData = ExtractAllData<T>(value);
        //            result.AddRange(subData);
        //        }
        //    }

        //    return result;
        //}
        public List<Dictionary<string, object>> ExtractAllData(object item)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;

                if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(DateTime))
                {
                    // Handle primitive types
                    // ...
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Handle collections of objects
                    var list = (IEnumerable)property.GetValue(item);

                    foreach (var listItem in list)
                    {
                        var nestedResults = ExtractAllData(listItem);

                        foreach (var nestedResult in nestedResults)
                        {
                            nestedResult.Add($"{property.Name}_{nestedResult.Keys.First()}", nestedResult.Values.First());
                            results.Add(nestedResult);
                        }
                    }
                }
                else
                {
                    // Handle nested objects
                    var nestedItem = property.GetValue(item);
                    var nestedResults = ExtractAllData(nestedItem);

                    foreach (var nestedResult in nestedResults)
                    {
                        nestedResult.Add($"{property.Name}_{nestedResult.Keys.First()}", nestedResult.Values.First());
                        results.Add(nestedResult);
                    }
                }
            }

            return results;
        }


        public List<Dictionary<string, object>> ExecuteQuery(string query, object id)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            using (var cmd = new SqlCommand(query, _connection))
            {
                if(id!= null)
                    cmd.Parameters.AddWithValue("@id", id);

                if (_connection.State != System.Data.ConnectionState.Open)
                    _connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dict.Add(reader.GetName(i), reader.GetValue(i));
                        }

                        result.Add(dict);
                    }
                }
            }

            return result;
        }
        public void PrintData(List<Dictionary<string, object>> data)
        {
            foreach (var obj in data)
            {
                foreach (var item in obj)
                {
                    Console.Write(item.Key + " : " + item.Value + " ");
                }
                Console.WriteLine();

            }
        }
    }
}
