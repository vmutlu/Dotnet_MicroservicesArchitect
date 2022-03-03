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
    public class AuctionClient
    {
        public HttpClient _client { get; }
        public AuctionClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.AuctionBaseAddress);
        }

        public async Task<Result<List<AuctionModel>>> GetAuctions()
        {
            var response = await _client.GetAsync("/api/v1/Auctions").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<List<AuctionModel>>(responseData);

                if (result.Any())
                    return new Result<List<AuctionModel>>(true, ResultConstant.RecordFound, result.ToList());

                return new Result<List<AuctionModel>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<AuctionModel>>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<AuctionModel>> CreateAuction(AuctionModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync("/api/v1/Auctions", content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<AuctionModel>(responseData);

                if (result is not null)
                    return new Result<AuctionModel>(true, ResultConstant.RecordCreateSuccessfully, result);

                else
                    return new Result<AuctionModel>(false, ResultConstant.RecordCreateNotSuccessfully);
            }
            return new Result<AuctionModel>(false, ResultConstant.RecordCreateNotSuccessfully);
        }

        public async Task<Result<AuctionModel>> GetAuctionById(string id)
        {
            var response = await _client.GetAsync("/api/v1/Auctions/" + id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<AuctionModel>(responseData);

                if (result is not null)
                    return new Result<AuctionModel>(true, ResultConstant.RecordFound, result);

                return new Result<AuctionModel>(false, ResultConstant.RecordNotFound);
            }
            return new Result<AuctionModel>(false, ResultConstant.RecordNotFound);
        }
    }
}
