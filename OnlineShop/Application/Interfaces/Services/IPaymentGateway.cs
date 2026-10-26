using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface IPaymentGateway
    {
        Task<PaymentRequestResult> RequestAsync(int amount, int orderId, string callbackUrl);

        Task<PaymentVerificationResult> VerifyAsync(int amount, string authority);
    }
}
