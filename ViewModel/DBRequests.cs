using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Banking.Model.Classes;
using Banking.DB;

namespace Banking.ViewModel
{
    internal class DBRequests
    {
        public async Task<List<Valute>> BDSelect()
        {
            List<Valute> list = new List<Valute>();
            using (SQLiteConnection connection = new SQLiteConnection(DataBase.GetConnection()))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(@"SELECT DateValute.Name, DateValute.Value, T.TypeOperation, DateValute.Date from DateValute INNER JOIN (SELECT ID, TypeOperation from Type) T ON T.ID = DateValute.TypeOperation", connection);
                SQLiteDataReader xRes = (SQLiteDataReader)await cmd.ExecuteReaderAsync();
                while (xRes.Read())
                {
                    Valute valute = new Valute()
                    {
                        Name = xRes.GetString(xRes.GetOrdinal("Name")),
                        Value = xRes.GetInt32(xRes.GetOrdinal("Value")),
                        Type = xRes.GetString(xRes.GetOrdinal("TypeOperation")),
                        Date = xRes.GetDateTime(xRes.GetOrdinal("Date"))
                    };
                    list.Add(valute);

                }
            }
            return list;

        }
        public async Task BDInsert(string Name, double Sum, string Type, DateTime dateNow)
        {
            int typeOperation = -1;
            using (SQLiteConnection connection = new SQLiteConnection(DataBase.GetConnection()))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(@"SELECT ID from Type WHERE TypeOperation = @Type", connection);
                cmd.Parameters.AddWithValue("@Type", Type);
                SQLiteDataReader xRes = (SQLiteDataReader)await cmd.ExecuteReaderAsync();
                while (xRes.Read())
                {
                    typeOperation = xRes.GetInt32(xRes.GetOrdinal("ID"));
                }

            }
            using (SQLiteConnection connection = new SQLiteConnection(DataBase.GetConnection()))
            {
                connection.Open();
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO DateValute (Name, Value, TypeOperation, Date) VALUES (@Name, @Sum, @TypeOperation, @DateNow)";
                    cmd.Parameters.AddWithValue("@TypeOperation", typeOperation);
                    cmd.Parameters.AddWithValue("@Sum", Sum);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@DateNow", dateNow);
                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }
    }
}
