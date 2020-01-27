namespace VirtualRoulette.DAL.DBProvider
{
    public interface IDBProvider
    {
        DataBase GetDBInstance();
    }

    public class DBProvider : IDBProvider
    {
        public DataBase GetDBInstance() => new DataBase();
    }
}
