using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Repositories
{
    public abstract class Repository
    {
        public static string ConnectionString { get; } = "Data Source=db-mssql;Initial Catalog=s17461;Integrated Security=True";
    }
}
