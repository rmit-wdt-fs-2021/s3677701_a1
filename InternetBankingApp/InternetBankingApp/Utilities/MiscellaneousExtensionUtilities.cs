using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace InternetBankingApp.Utilities
{
    /// <summary>
    /// Utilities class referenced from TuteLab03.
    /// </summary>
    public static class MiscellaneousExtensionUtilities
    {
        public static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;

        public static SqlConnection CreateConnection(this string connectionString) =>
            new SqlConnection(connectionString);

        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            try
            {
                new SqlDataAdapter(command).Fill(table);
            }catch(SqlException e)
            {
                Console.WriteLine("Unable to connect to Azure SQL Server at this moment. Please try again.");
                throw;
            }catch(Exception e)
            {
                Console.WriteLine("Something has gone wrong on our end. Please try again later.");
                Console.WriteLine(e.Message);
                throw;
            }
            return table;
        }

        public static async Task<DataTable> GetDataTableAsync(this SqlCommand command)
        {
            var table = new DataTable();
            var adapter = new SqlDataAdapter(command);

            await Task.Run(() => adapter.Fill(table));
            return table;
        }

        public static decimal RoundUp(this decimal number, int places = 2) => Math.Round(number, places);
    }
}
