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
    public class BidClient
    {
        public HttpClient _client { get; }
        public BidClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.AuctionBaseAddress);
        }

        public async Task<Result<List<BidModel>>> GetBidsByAuctionId(string id)
        {
            var response = await _client.GetAsync("/api/v1/Bids/GetBidsByAuctionId?id=" + id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<List<BidModel>>(responseData);

                if (result.Any())
                    return new Result<List<BidModel>>(true, ResultConstant.RecordFound, result.ToList());

                return new Result<List<BidModel>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<BidModel>>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<List<BidModel>>> GetAllBidsByAuctionId(string id)
        {
            var response = await _client.GetAsync("/api/v1/Bids/GetAllBidsByAuctionId?id=" + id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<List<BidModel>>(responseData);

                if (result.Any())
                    return new Result<List<BidModel>>(true, ResultConstant.RecordFound, result.ToList());

                return new Result<List<BidModel>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<BidModel>>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<string>> SendBid(BidModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync("/api/v1/Bids", content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new Result<string>(true, ResultConstant.RecordCreateSuccessfully, responseData);
            }

            return new Result<string>(false, ResultConstant.RecordCreateNotSuccessfully);
        }
    }
}
