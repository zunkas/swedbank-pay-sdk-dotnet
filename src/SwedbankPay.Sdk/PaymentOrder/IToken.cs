namespace SwedbankPay.Sdk.PaymentOrder;

public interface IToken
{
   string? Token { get; init; }
   string? TokenType { get; init; }
   string? Instrument { get; init; }
   string? InstrumentDisplayName { get; init; }
   string? CorrelationId { get; init; }
   IInstrumentParameters? InstrumentParameters { get; init; }
   IOperationList? Operations { get; init; }
}