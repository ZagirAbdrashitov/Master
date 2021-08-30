using Microsoft.AspNetCore.Mvc;
using DrugsManager.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DrugsManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DrugsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var query = @"select * from dbo.Drug";
            var table = new DataTable();
            SqlDataReader sqlReader;
            var connectionString = _configuration.GetConnectionString("DrugsManagerCon");
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Drug drug)
        {
            var query = @"insert into dbo.Drug (Ndc, Name, PackSize, Unit, Price) values (@Ndc, @Name, @PackSize, @Unit, @Price)";
            var table = new DataTable();
            SqlDataReader sqlReader;
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DrugsManagerCon")))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Ndc", drug.Ndc);
                    sqlCommand.Parameters.AddWithValue("@Name", drug.Name);
                    sqlCommand.Parameters.AddWithValue("@PackSize", drug.PackSize);
                    sqlCommand.Parameters.AddWithValue("@Unit", (int)drug.Unit);
                    sqlCommand.Parameters.AddWithValue("@Price", drug.Price);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Drug drug)
        {
            var query = @"update dbo.Drug set Ndc=@Ndc, Name=@Name, PackSize=@PackSize, Unit=@Unit, Price=@Price where Id=@Id";
            var table = new DataTable();
            SqlDataReader sqlReader;
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DrugsManagerCon")))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", drug.Id);
                    sqlCommand.Parameters.AddWithValue("@Ndc", drug.Ndc);
                    sqlCommand.Parameters.AddWithValue("@Name", drug.Name);
                    sqlCommand.Parameters.AddWithValue("@PackSize", drug.PackSize);
                    sqlCommand.Parameters.AddWithValue("@Unit", (int)drug.Unit);
                    sqlCommand.Parameters.AddWithValue("@Price", drug.Price);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var query = @"delete from dbo.Drug where Id=@Id";
            var table = new DataTable();
            SqlDataReader sqlReader;
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DrugsManagerCon")))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
