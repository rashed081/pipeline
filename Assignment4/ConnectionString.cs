using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class ConnectionString
    {
        public static SqlConnection _connection = new SqlConnection("Server=DESKTOP-86V0L02;Database=assignment_4;Trusted_Connection=True;");
    }
}
