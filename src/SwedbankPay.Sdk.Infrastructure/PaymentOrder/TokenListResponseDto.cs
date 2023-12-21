namespace SwedbankPay.Sdk.Infrastructure.PaymentOrder;

internal record TokenListResponseDto
{
    public TokenResponseDto PayerOwnedTokens { get; init; } = null!;
    public OperationListDto Operations { get; init; } = null!;

    public TokenListResponse Map(HttpClient httpClient)
    {
        return new TokenListResponse(this, httpClient);
    }
}
