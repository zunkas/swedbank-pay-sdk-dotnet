using SwedbankPay.Sdk.PaymentOrder;

namespace SwedbankPay.Sdk.Infrastructure.PaymentOrder;

internal record UserTokenListResponse : IUserTokenResponse
{
    public UserTokenListResponse(UserTokenListResponseDto userTokenListResponseDto, HttpClient httpClient)
    {
        Id = userTokenListResponseDto.PayerOwnedTokens.Id;
        PayerReference = userTokenListResponseDto.PayerOwnedTokens.PayerReference;
        
        Tokens = userTokenListResponseDto.PayerOwnedTokens.Tokens?.Select(x => x.Map()).ToList();
        Operations = new UserTokenOperations(userTokenListResponseDto.Operations.Map(), httpClient);
    }

    public string? Id { get; }
    public string? PayerReference { get; }
    public List<IUserToken>? Tokens { get; }
    
    public IUserTokenOperations? Operations { get;}


}