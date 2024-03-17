using System;
using SQLitePCL;
using System.IO;
using Windows.Storage;
using System.Data.SQLite;

namespace Banking.DB
{
    internal class DataBase
    {




        private static string localFolder = ApplicationData.Current.LocalFolder.Path;
        public string dbPath = Path.Combine(localFolder, "BDValute.db");

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={dbPath}; Read Only=False;");
        }

            
           
    }
}

