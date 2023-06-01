using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.Common;

namespace Webshop.Service.CustomerClient.Models.Responses
{
    public class CustomerResponse: BaseResponse
    {
        public CustomerDto Result { get; set; }
    }
}
