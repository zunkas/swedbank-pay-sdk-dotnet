namespace SwedbankPay.Sdk.Infrastructure.PaymentOrder;

internal record UserTokenListResponseDto
{
    public UserTokenResponseDto PayerOwnedTokens { get; init; } = null!;
    public OperationListDto Operations { get; init; } = null!;

    public UserTokenListResponse Map(HttpClient httpClient)
    {
        return new UserTokenListResponse(this, httpClient);
    }
}
