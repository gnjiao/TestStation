using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Database
{
    public class DatabaseSrv
    {
        #region singleton
        protected DatabaseSrv()
        {
            Logger log = new Logger(typeof(DatabaseSrv));
            log.Debug(string.Format("DatabaseSrv(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
        private static DatabaseSrv _instance;
        public static DatabaseSrv GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseSrv();
            }
            return _instance;
        }
        #endregion

        public void Save(Dictionary<string, string> kvs)
        {
        }
        public DataTable Load()
        {
            return new DataTable();
        }
    }
}
