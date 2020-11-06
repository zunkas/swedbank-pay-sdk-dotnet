﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Sample.AspNetCore.Models;

using SwedbankPay.Sdk;
using SwedbankPay.Sdk.PaymentInstruments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.AspNetCore
{
    public static class PaymentHelper
    {
        public static async Task CancelCreditCardPayment(string paymentId,
                                                         ISwedbankPayClient swedbankPayClient,
                                                         ITempDataDictionary tempData,
                                                         Cart cartService)
        {
            var payment = await swedbankPayClient.Payments.CardPayments.Get(new Uri(paymentId, UriKind.RelativeOrAbsolute));   

            if (payment.Operations.Cancel != null)
            {
                var cancelRequest = new SwedbankPay.Sdk.PaymentInstruments.Card.CardPaymentCancelRequest(DateTime.Now.Ticks.ToString(), "Cancelling parts of the total amount");
                var response = await payment.Operations.Cancel(cancelRequest);
                tempData["CancelMessage"] = $"Payment has been cancelled: {response.Cancellation.Transaction.Id}";
                cartService.PaymentOrderLink = null;
            }
            else
            {
                tempData["ErrorMessage"] = "Operation not available";
            }
        }

        public static async Task CancelTrustlyPayment(string paymentId, 
                                                                            ISwedbankPayClient swedbankPayClient, 
                                                                            ITempDataDictionary tempData,
                                                                            Cart cartService)
        {
            var payment = await swedbankPayClient.Payments.TrustlyPayments.Get(new Uri(paymentId, UriKind.RelativeOrAbsolute));

            if (payment.Operations.Cancel != null)
            {
                var cancelRequest = new SwedbankPay.Sdk.PaymentInstruments.Trustly.TrustlyPaymentCancelRequest(DateTime.Now.Ticks.ToString(), "Cancelling parts of the total amount");
                var response = await payment.Operations.Cancel(cancelRequest);
                tempData["CancelMessage"] = $"Payment has been cancelled: {response.Cancellation.Transaction.Id}";
                cartService.PaymentOrderLink = null;
            }
            else
            {
                tempData["ErrorMessage"] = "Operation not available";
            }
        }


        public static async Task<IReversalResponse> ReverseSwishPayment(string paymentId, Order order, string description, ISwedbankPayClient swedbankPayClient)
        {
            var swishPayment = await swedbankPayClient.Payments.SwishPayments.Get(new Uri(paymentId, UriKind.RelativeOrAbsolute));
            var swishReversal = new SwedbankPay.Sdk.PaymentInstruments.Swish.SwishPaymentReversalRequest(
                new Amount(order.Lines.Sum(e => e.Quantity * e.Product.Price)),
                new Amount(0), description, DateTime.Now.Ticks.ToString());
            return await swishPayment.Operations.Reverse.Invoke(swishReversal);
        }


        public static async Task<IReversalResponse> ReverseCreditCardPayment(string paymentId, Order order, string description, ISwedbankPayClient swedbankPayClient)
        {
            var cardPayment = await swedbankPayClient.Payments.CardPayments.Get(new Uri(paymentId, UriKind.RelativeOrAbsolute));
            var cardReversal = new SwedbankPay.Sdk.PaymentInstruments.Card.CardPaymentReversalRequest
            {
                Amount = new Amount(order.Lines.Sum(e => e.Quantity * e.Product.Price)),
                VatAmount = new Amount(0),
                Description = description,
                PayeeReference = DateTime.Now.Ticks.ToString()
            };
            return await cardPayment.Operations.Reverse.Invoke(cardReversal);
        }


        public static async Task<IReversalResponse> ReverseTrustlyPayment(string paymentId, Order order, string description, ISwedbankPayClient swedbankPayClient)
        {
            var trustlyPayment = await swedbankPayClient.Payments.TrustlyPayments.Get(new Uri(paymentId, UriKind.RelativeOrAbsolute));
            var trustlyReversal = new SwedbankPay.Sdk.PaymentInstruments.Trustly.TrustlyPaymentReversalRequest(Operation.Sale,
                                                                                                               new Amount(order.Lines.Sum(e => e.Quantity * e.Product.Price)),
                                                                                                               new Amount(0),
                                                                                                               DateTime.Now.Ticks.ToString(),
                                                                                                               "receipt reference",
                                                                                                               description);
            return await trustlyPayment.Operations.Reverse.Invoke(trustlyReversal);
        }
    }
}
