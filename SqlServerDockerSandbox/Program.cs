using System;
using System.Data.SqlClient;

namespace SqlServerDockerSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
			// See https://stackoverflow.com/questions/17157721/how-to-get-a-docker-containers-ip-address-from-the-hostto get docker container ip
			// However for Mac just use localhost.  See https://docs.docker.com/docker-for-mac/networking/#use-cases-and-workarounds 
			// See https://stackoverflow.com/questions/1975780/sql-server-enable-remote-connections-without-ssms to allow remote connections.  Must restart mssql or the container
			// See https://github.com/dotnet/cli/issues/5129 for openssl issue
			/*
                sudo install_name_tool -add_rpath /usr/local/opt/openssl/lib /usr/local/share/dotnet/shared/Microsoft.NETCore.App/1.1.1/System.Security.Cryptography.Native.OpenSsl.dylib
                sudo install_name_tool -add_rpath /usr/local/opt/openssl/lib /usr/local/share/dotnet/shared/Microsoft.NETCore.App/2.0.0-preview1-002111-00/System.Security.Cryptography.Native.OpenSsl.dylib
             */
			using (var connection = new SqlConnection("Server=tcp:localhost,1433;Initial Catalog=master;MultipleActiveResultSets=true;User Id=sa;Password=Panama\\!123"))
			{
				var command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", connection);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Console.WriteLine($"{reader[0]}");
					}
				}
			}
			Console.WriteLine("Press any key to terminate");
            Console.ReadKey();
		}
    }
}
