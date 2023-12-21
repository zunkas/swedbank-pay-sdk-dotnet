namespace SwedbankPay.Sdk.PaymentOrder;

public interface ITokenResponse
{
   string? Id { get; }
   string? PayerReference { get; }
   List<IToken>? Tokens { get; }
   
   ITokenOperations? Operations { get;}
}