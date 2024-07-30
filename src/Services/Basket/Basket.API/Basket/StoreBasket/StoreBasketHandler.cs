﻿
using Basket.API.Data;
using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(c=>c.Cart).NotNull().WithMessage("Cart can not be null!");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required!");
        }
    }

    public class StoreBasketCommandHandler
        (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) 
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command, cancellationToken);

            await repository.StoreBasketAsync(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task DeductDiscount(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            foreach (var item in command.Cart.Items)
            {
                var coupon = await discountProto
                    .GetDiscountAsync(new GetDiscountRequest() { ProductName = item.ProductName }, cancellationToken: cancellationToken);

                item.Price -= coupon.Amount;
            }
        }
    }
}
