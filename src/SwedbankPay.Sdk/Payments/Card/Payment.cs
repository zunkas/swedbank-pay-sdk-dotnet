﻿using SwedbankPay.Sdk.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwedbankPay.Sdk.Payments.Card
{
    public class Payment
    {
        private Payment(PaymentResponse paymentResponse, HttpClient client)
        {
            PaymentResponse = paymentResponse.Payment;
            var operations = new Operations();

            foreach (var httpOperation in paymentResponse.Operations)
            {
                operations.Add(httpOperation.Rel, httpOperation);

                switch (httpOperation.Rel.Value)
                {
                    case PaymentResourceOperations.UpdatePaymentAbort:
                        operations.Update = httpOperation;
                        break;

                    case PaymentResourceOperations.RedirectAuthorization:
                        operations.RedirectAuthorization = httpOperation;
                        break;

                    case PaymentResourceOperations.ViewAuthorization:
                        operations.ViewAuthorization = httpOperation;
                        break;

                    case PaymentResourceOperations.DirectAuthorization:
                        operations.DirectAuthorization = async payload =>
                            await client.PostAsJsonAsync<AuthorizationResponse>(httpOperation.Href, payload);
                        break;

                    case PaymentResourceOperations.CreateCapture:
                        operations.Capture = async payload =>
                            await client.PostAsJsonAsync<CaptureResponse>(httpOperation.Href,payload);
                        break;

                    case PaymentResourceOperations.CreateCancellation:
                        operations.Cancel = async payload =>
                            await client.PostAsJsonAsync<CancellationResponse>(httpOperation.Href, payload);
                        break;

                    case PaymentResourceOperations.CreateReversal:
                        operations.Reversal = async payload =>
                            await client.PostAsJsonAsync<ReversalResponse>(httpOperation.Href, payload);
                        break;

                    case PaymentResourceOperations.RedirectVerification:
                        operations.RedirectVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.ViewVerification:
                        operations.ViewVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.DirectVerification:
                        operations.DirectVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.PaidPayment:
                        operations.PaidPayment = httpOperation;
                        break;
                }
            }

            Operations = operations;
        }


        public Operations Operations { get; }

        public PaymentResponseObject PaymentResponse { get; }


        internal static async Task<Payment> Create(PaymentRequest paymentRequest,
                                                   HttpClient client,
                                                   string paymentExpand)
        {
            var url = new Uri($"/psp/creditcard/payments{paymentExpand}", UriKind.Relative);

            var paymentResponse = await client.PostAsJsonAsync<PaymentResponse>(url, paymentRequest);
            return new Payment(paymentResponse, client);
        }


        internal static async Task<Payment> Get(Uri id, HttpClient client, string paymentExpand)
        {
            var url = !string.IsNullOrWhiteSpace(paymentExpand)
                ? new Uri(id.OriginalString + paymentExpand, UriKind.RelativeOrAbsolute)
                : id;

            var paymentResponseContainer = await client.GetAsJsonAsync<PaymentResponse>(url);
            return new Payment(paymentResponseContainer, client);
        }
    }
}