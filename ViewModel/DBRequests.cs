using Banking.DB;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using static Banking.Model.Classes;

namespace Banking.ViewModel
{
    internal class DBRequests
    {
        DataBase db = new DataBase();
        public async void BDCreate()
        {
            if (!File.Exists(db.dbPath))
            {
                using (SQLiteConnection connection = new SQLiteConnection(db.GetConnection()))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE IF NOT EXISTS Type (
                                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            TypeOperation TEXT
                                        );";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"CREATE TABLE IF NOT EXISTS DateValute (
                                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT,
                                            Value INTEGER,
                                            Date INTEGER,
                                            TypeOperation INTEGER, FOREIGN KEY(TypeOperation) REFERENCES Type(ID)   
                                        );";
                        await command.ExecuteNonQueryAsync();


                        command.CommandText = @"INSERT INTO Type (TypeOperation) VALUES (""Зачисление"")";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"INSERT INTO Type (TypeOperation) VALUES (""Снятие"")";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

        }
        public async Task<List<Valute>> BDSelect()
        {
            List<Valute> list = new List<Valute>();
            using (SQLiteConnection connection = new SQLiteConnection(db.GetConnection()))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(@"SELECT DateValute.Name, DateValute.Value, T.TypeOperation, DateValute.Date from DateValute 
                INNER JOIN (SELECT ID, TypeOperation from Type) T ON T.ID = DateValute.TypeOperation", connection);
                SQLiteDataReader xRes = (SQLiteDataReader)await cmd.ExecuteReaderAsync();
                while (xRes.Read())
                {
                    Valute valute = new Valute()
                    {
                        Name = xRes.GetString(xRes.GetOrdinal("Name")),
                        Value = xRes.GetDouble(xRes.GetOrdinal("Value")),
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
            using (SQLiteConnection connection = new SQLiteConnection(db.GetConnection()))
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
            using (SQLiteConnection connection = new SQLiteConnection(db.GetConnection()))
            {
                connection.Open();
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO DateValute (Name, Value, Date, TypeOperation) VALUES (@Name, @Sum, @DateNow, @TypeOperation)";
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
