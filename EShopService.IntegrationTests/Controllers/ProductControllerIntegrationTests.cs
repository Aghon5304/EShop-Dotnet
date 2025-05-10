using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace EShopService.IntegrationTests.Controllers;

public class ProductControllerIntegrationTests:IClassFixture<WebApplicationFactory<Program>>
{
	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _client;
	public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();
	}
	//[Fact]
	//public async Task Get_GetAllProducts_ReturnsOk()
	//{
	//	// arrange
	//	var request = new 
	//}
}

