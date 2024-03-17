using System.Data.SQLite;
using System.IO;
using Windows.Storage;

namespace Banking.DB
{
    static class DataBase
    {
        private static string localFolder = ApplicationData.Current.LocalFolder.Path;
        private static string dbPath = Path.Combine(localFolder, "BDValute.db");

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={dbPath}; Read Only=False;");
        }
    }

}
