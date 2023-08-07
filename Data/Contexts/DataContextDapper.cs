using System.Data;
using Dapper;
using DotnetAPI.Dtos;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data {
    class DataContextDapper {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config) {
            _config = config;
        }

        public IEnumerable<T> loadData<T>(string query){
            
            IEnumerable<T>? data;
            using(IDbConnection dBConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"))){
                data = dBConnection.Query<T>(query);
            };
            return data;
        }

        public T? loadDataSingle<T>(string query){      
            T? data;
            using(IDbConnection dBConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"))){
                data = dBConnection.QuerySingleOrDefault<T?>(query);
            };
            return data;
        }

        public bool ExecuteQuery (string sql, Object? parameters = null) {
            // Create a SqlConnection to action
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    // Open the database connection
                    dbConnection.Open();
                    // Execute the INSERT statement using Dapper
                    return dbConnection.Execute(sql,parameters) > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
    }
}