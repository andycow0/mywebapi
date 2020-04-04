using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mywebapi.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new [] {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController (ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get () {
            var rng = new Random ();
            var param = String.Empty;
            using (var conn = new SqlConnection ("Data Source=ATE_SERVER_T3;Initial Catalog=ate_db;User ID=ate_oper; Password=ate.oper"))
            using (var cmd = new SqlCommand ()) {

                var sql = new StringBuilder ();
                sql.Append (" select top 100 parameter, ParameterIndex   ");
                sql.Append (" from pack_oneyear_db..tblcontrolparameter with(nolock)");
                sql.Append (" where  ControlItem ='Bty2' ");

                // var sqlParams = new SqlParameter[2] {
                //     new SqlParameter () { ParameterName = "@GPN", Value = autoScrewResultSearchCriteria.GPN, SqlDbType = SqlDbType.VarChar, Size = 20 },
                //     new SqlParameter () { ParameterName = "@Status", Value = autoScrewResultSearchCriteria.Status, SqlDbType = SqlDbType.Int },
                // };
                // cmd.Parameters.AddRange (sqlParams);
                cmd.CommandText = sql.ToString ();
                cmd.Connection = conn;
                conn.Open ();

                using (SqlDataReader reader = cmd.ExecuteReader ()) {
                    while (reader.Read ()) {
                        // Console.WriteLine ("{0} ", reader["parameter"].ToString ());
                        param = reader["parameter"].ToString ();
                    }
                }
            }

            Console.WriteLine ($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} WeatherForecast - Get().");
            return Enumerable.Range (1, 5).Select (index => new WeatherForecast {
                    Date = DateTime.Now.AddDays (index),
                        TemperatureC = rng.Next (-20, 55),
                        Summary = Summaries[rng.Next (Summaries.Length)],
                        Test = param
                })
                .ToArray ();
        }
    }
}