using SwedbankPay.Sdk.PaymentOrder;

namespace SwedbankPay.Sdk.Infrastructure.PaymentOrder;

internal record TokenListResponse : ITokenResponse
{
    public TokenListResponse(TokenListResponseDto tokenListResponseDto, HttpClient httpClient)
    {
        Id = tokenListResponseDto.PayerOwnedTokens.Id;
        PayerReference = tokenListResponseDto.PayerOwnedTokens.PayerReference;
        // Tokens = new List<IToken>(tokenListResponseDto.PayerOwnedTokens.Tokens!);
        
        // IToken t = new TokenDto()
        Operations = new TokenOperations(tokenListResponseDto.Operations.Map(), httpClient);
    }

    public string? Id { get; }
    public string? PayerReference { get; }
    public List<IToken>? Tokens { get; }
    
    public ITokenOperations? Operations { get;}


}