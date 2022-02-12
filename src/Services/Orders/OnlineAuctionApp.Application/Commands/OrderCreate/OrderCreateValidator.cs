using FluentValidation;

namespace OnlineAuctionApp.Application.Commands.OrderCreate
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateValidator()
        {
            RuleFor(o => o.SellerUserName).EmailAddress().NotEmpty();
            RuleFor(o => o.ProductId).NotEmpty();
        }
    }
}
