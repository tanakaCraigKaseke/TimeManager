using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.MainLibrary.Helpers
{
    public class DataResponse
    {
        public bool IsSuccsesful { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
