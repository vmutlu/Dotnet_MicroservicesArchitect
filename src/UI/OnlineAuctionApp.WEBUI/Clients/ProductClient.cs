using Newtonsoft.Json;
using OnlineAuctionApp.Core.Commons;
using OnlineAuctionApp.Core.Response.Concrete;
using OnlineAuctionApp.WEBUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineAuctionApp.WEBUI.Clients
{
    public class ProductClient
    {
        public HttpClient _client { get; }

        public ProductClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.ProductBaseAddress);
        }

        public async Task<Result<List<ProductModel>>> GetProducts()
        {
            var response = await _client.GetAsync("/api/v1/Products").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);
                if (result.Any())
                    return new Result<List<ProductModel>>(true, ResultConstant.RecordFound, result.ToList());

                return new Result<List<ProductModel>>(false, ResultConstant.RecordNotFound);
            }

            return new Result<List<ProductModel>>(false, ResultConstant.RecordNotFound);
        }
    }
}
