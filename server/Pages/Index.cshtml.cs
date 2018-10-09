using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using System.Text;

namespace HelloWorldTest.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public string Message2 { get; set; }

        public void OnGet()
        {
            string first_name;
            string last_name;
            string genre;
            string instrument;
            string message;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(localdb)\\ProjectsV13";
            builder.InitialCatalog = "pluggedin_localdb";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT first_name, last_name, genre, instrument ");
                sb.Append("FROM musicians ");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            first_name = reader.GetString(0);
                            last_name = reader.GetString(1);
                            genre = reader.GetString(2);
                            instrument = reader.GetString(3);

                            message = first_name + " " + last_name + " a " + genre + " a musician who plays the " + instrument;
                            Console.WriteLine(message);
                            //The above writes to the App stdout.

                            //await context.Response.WriteAsync("Hello World! \n" + message);
                            Message = "Hello World!";
                            Message2 = message;
                        }
                    }
                }
            }
        }
    }
}
