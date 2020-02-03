﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SwedbankPay.Sdk.Exceptions;

namespace SwedbankPay.Sdk.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, Uri uri)
        {
            var apiResponse = await httpClient.GetAsync(uri);

            var responseString = await apiResponse.Content.ReadAsStringAsync();

            if (!apiResponse.IsSuccessStatusCode)
                throw new HttpResponseException(
                    apiResponse,
                    JsonConvert.DeserializeObject<ProblemResponse>(responseString),
                    BuildErrorMessage(responseString, uri, apiResponse));

            
            return JsonConvert.DeserializeObject<T>(responseString, JsonSerialization.JsonSerialization.Settings);
        }

        public static Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string uri)
        {
            var requestUri = new Uri(uri);
            return httpClient.GetAsJsonAsync<T>(requestUri);
        }

        internal static async Task<T> SendAndProcessAsync<T>(this HttpClient httpClient, HttpMethod httpMethod, Uri uri, object payload)
            where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);

            if (payload != null)
            {
                var content = JsonConvert.SerializeObject(payload, JsonSerialization.JsonSerialization.Settings);
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            var httpResponse = await httpClient.SendAsync(httpRequestMessage);

            try
            {
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                if (!httpResponse.IsSuccessStatusCode)
                    throw new HttpResponseException(
                        httpResponse,
                        JsonConvert.DeserializeObject<ProblemResponse>(httpResponseBody),
                        BuildErrorMessage(httpResponseBody, httpRequestMessage, httpResponse));
                return JsonConvert.DeserializeObject<T>(httpResponseBody, JsonSerialization.JsonSerialization.Settings);
            }
            catch (HttpResponseException ex)
            {
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                throw new HttpResponseException(
                    httpResponse,
                    message: BuildErrorMessage(httpResponseBody, httpRequestMessage, httpResponse),
                    innerException: ex);
            }
        }


        private static string BuildErrorMessage(string httpResponseBody, HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        {
            return
                $"{httpRequest.Method}: {httpRequest.RequestUri} failed with error code {httpResponse.StatusCode} using bearer token {httpRequest.Headers.Authorization.Parameter}. Response body: {httpResponseBody}";
        }

        private static string BuildErrorMessage(string httpResponseBody, Uri uri, HttpResponseMessage httpResponse)
        {
            return
                $"GET: {uri} failed with error code {httpResponse.StatusCode}. Response body: {httpResponseBody}";
        }

        public static Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, Uri uri, object payload)
            where T : class
        {
            return httpClient.SendAndProcessAsync<T>(HttpMethod.Post, uri, payload);
        }

        public static Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string url, object payload)
            where T : class
        {
            var requestUri = new Uri(url);
            return httpClient.SendAndProcessAsync<T>(HttpMethod.Post, requestUri, payload);
        }


        public static Task<T> PutAsJsonAsync<P, T>(this HttpClient httpClient, Uri uri, object payload)
            where T : class
        {
            return httpClient.SendAndProcessAsync<T>(HttpMethod.Put, uri, payload);
        }

        public static Task<T> PutAsJsonAsync<T>(this HttpClient httpClient, string uri, object payload)
            where T : class
        {
            var requestUri = new Uri(uri);
            return httpClient.SendAndProcessAsync<T>(HttpMethod.Put, requestUri, payload);
        }


        public static Task<T> PatchAsJsonAsync<T>(this HttpClient httpClient, Uri uri, object payload)
            where T : class
        {
            return httpClient.SendAndProcessAsync<T>(new HttpMethod("PATCH"), uri, payload);
        }

        public static Task<T> PatchAsJsonAsync<T>(this HttpClient httpClient, string uri, object payload)
            where T : class
        {
            var requestUri = new Uri(uri);
            return httpClient.SendAndProcessAsync<T>(new HttpMethod("PATCH"), requestUri, payload);
        }
    }
}
