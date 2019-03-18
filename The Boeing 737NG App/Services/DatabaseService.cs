using System.Collections.Generic;
using SQLite;

namespace The_Boeing_737NG_App.Services
{
    public interface IDatabaseService
    {
        void CreateTable<T>();
        void DropTable<T>();
        void ClearTable<T>();
        void Insert<T>(object item);
        void Delete(object item);
        List<T> SelectTableAsList<T>() where T : new();
    }
    public class DatabaseService : IDatabaseService
    {
        SQLiteConnection _sQLiteConnection;

        public static DatabaseService Instance(string filePath) => new DatabaseService(filePath);              

        public DatabaseService(string filePath) => _sQLiteConnection = new SQLiteConnection(filePath);

        public void CreateTable<T>() => _sQLiteConnection.CreateTable<T>();

        public void Insert<T>(object item) => _sQLiteConnection.Insert((T)item);

        public void DropTable<T>() => _sQLiteConnection.DropTable<T>();

        public List<T> SelectTableAsList<T>() where T : new() => _sQLiteConnection.Table<T>().ToList();

        public void ClearTable<T>()
        {
            _sQLiteConnection.DeleteAll<T>();
            _sQLiteConnection.ExecuteScalar<T>("UPDATE SQLITE_SEQUENCE SET SEQ=0", 0);
        }

        public void Delete(object item)
        {
            _sQLiteConnection.Delete(item);
        }
    }
}