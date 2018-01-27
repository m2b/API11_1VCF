using System;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;

namespace APIVCF
{
    class TankRepositorySandbox
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting Local Path");

			string dir = Path.GetDirectoryName(new Uri(typeof(TankRepositorySandbox).GetTypeInfo().Assembly.CodeBase).LocalPath);

			Console.WriteLine("Creating Repository");

            RdbmsTankDataRepository<SqlConnection> repos = new RdbmsTankDataRepository<SqlConnection>(dir);

			Console.WriteLine("Repository Created");

            Console.WriteLine("Press any key to proceed to delete database");
            Console.ReadKey();

			using (var connection = new SqlConnection("Server=tcp:localhost,1433;MultipleActiveResultSets=true;User Id=sa;Password=Panama\\!123"))
			{
				var command = new SqlCommand("DROP DATABASE TankData", connection);
				connection.Open();
                command.ExecuteNonQuery();
			}

            Console.WriteLine("Database dropped");

			Console.WriteLine("Press any key to end");
			Console.ReadKey();
		}
    }
}
