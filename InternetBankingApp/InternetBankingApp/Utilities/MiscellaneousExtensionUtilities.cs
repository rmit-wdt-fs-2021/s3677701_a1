using System;
using System.Data;
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
            new SqlDataAdapter(command).Fill(table);

            return table;
        }

        public static decimal RoundUp(this decimal number, int places = 2) => Math.Round(number, places);
    }
}
