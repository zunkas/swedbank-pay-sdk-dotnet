namespace SwedbankPay.Sdk.PaymentOrder;

public class PaymentOrderAbortRequestDetail
{
    /// <summary>
    /// Instantiates a <see cref="PaymentOrderAbortRequestDetail"/> with default values.
    /// </summary>
    internal PaymentOrderAbortRequestDetail()
    {
        AbortReason = "CancelledByConsumer";
    }

    /// <summary>
    /// The reason why the current payment is being aborted.
    /// </summary>
    public string AbortReason { get; set; }

    /// <summary>
    /// The Api operation.
    /// This is set to "Abort".
    /// </summary>
    public string Operation { get; } = "Abort";
}