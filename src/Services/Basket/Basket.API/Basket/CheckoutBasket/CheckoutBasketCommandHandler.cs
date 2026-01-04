using Basket.API.Data;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;

    public record CheckoutBasketResult(bool IsSuccess);

    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto cannot be null");
            RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketCommandHandler : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasketAsync(command.BasketCheckout.UserName, cancellationToken);
            if (basket is null)
                return new CheckoutBasketResult(false);

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage, cancellationToken);

            await _repository.DeleteBasketAsync(command.BasketCheckout.UserName, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}
