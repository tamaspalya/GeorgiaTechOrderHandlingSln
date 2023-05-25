using System;
namespace Webshop.Services.CustomerService.Models.Responses
{
	public class GetCustomerResponse
	{
            public CustomerDto? Result { get; set; }
			public string? ErrorMessage { get; set; }
			public string? TimeGenerated { get; set; }
	}
}

