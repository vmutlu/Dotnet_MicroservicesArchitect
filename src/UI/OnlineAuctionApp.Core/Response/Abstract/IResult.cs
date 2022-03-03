namespace OnlineAuctionApp.Core.Response.Abstract
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
