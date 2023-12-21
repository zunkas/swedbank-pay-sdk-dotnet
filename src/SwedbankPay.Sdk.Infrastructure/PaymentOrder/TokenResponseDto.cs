using SwedbankPay.Sdk.PaymentOrder;

namespace SwedbankPay.Sdk.Infrastructure.PaymentOrder;
internal record TokenResponseDto
{
    public string? Id { get; init; }
    public string? PayerReference { get; init; }
    public List<TokenDto>? Tokens { get; init; }
    public TokenOperations? Operations { get; init; }
}