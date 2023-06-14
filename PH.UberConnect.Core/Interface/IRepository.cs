using PH.UberConnect.Core.Connection;

namespace PH.UberConnect.Core.Interface
{
    public interface IRepository
    {
        //public static string ServerName => "[ServerPruebas].[ClickEat]";
        public static string ServerName => "[PHCRSRVMCARIIS0].[ClickEat]";
        public string Table { get; }
        public IConnection Connection { get; }
    }
}
