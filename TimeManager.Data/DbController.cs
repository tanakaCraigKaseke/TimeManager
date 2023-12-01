using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Data
{
    public static class DbController
    {
        public   const string ConnectionString = "Server=tcp:time-manager-server.database.windows.net,1433;Initial Catalog=timeManager;Persist Security Info=False;User ID=TimeManager;Password=formula.1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    }
}
