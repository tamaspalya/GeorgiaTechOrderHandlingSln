using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.Common
{
    public abstract class BaseResponse
    {
        public string ErrorMessage { get; set; }
        public string TimeGenerated { get; set; }
    }
}
