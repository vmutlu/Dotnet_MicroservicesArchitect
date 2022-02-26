namespace OnlineAuctionApp.Core.Entities
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Seller or Buyer Users
        public bool IsSeller { get; set; }
        public bool IsBuyer { get; set; }
    }
}
