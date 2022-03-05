
using LearningContest.Application.Contracts.HttpCall;
using LearningContest.Application.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LearningContest.Domain.Common;

namespace LearningContest.Infrastructure.HttpCall
{

    public class HttpCallService : IHttpCallService
    {
        public async Task<(T Result, ApiCallResponseError Errors)> PostAlborzServiceAsync<T>(string uri, object model,
            Dictionary<string, string> headers = null, int timeout = 120000)
        {
            return await PostAlborzServiceAsyncPrivate<T>(uri, model, headers, timeout);
        }
        public async Task<(T Result, ApiCallResponseError Errors)> GetAlborzServiceAsync<T>(string uri, Dictionary<string, string> headers, int timeout = 120000)
        {
            return await GetAlborzServiceAsyncPrivate<T>(uri, headers, timeout);
        }
        private async Task<(T Result, ApiCallResponseError Errors)> GetAlborzServiceAsyncPrivate<T>(string uri, Dictionary<string, string> headers, int timeout = 120000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = timeout;

            if (headers != null)
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = await reader.ReadToEndAsync();


                    return (Result: JsonConvert.DeserializeObject<T>(data), Errors: null);

                }
            }
            catch (WebException e)
            {
                return (Result: default(T), Errors: await handleError(e));
            }
        }

        private async Task<ApiCallResponseError> handleError(WebException e)
        {
            try
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {

                        var result = await reader.ReadToEndAsync();
                        var deserilizeResult = JsonConvert.DeserializeObject<ApiCallResponseError>(result);
                        return deserilizeResult;
                    }

                }
            }

            catch (Exception ex)
            {
                Log.Error(ex, "خطای نامشخص");
                return new ApiCallResponseError();
            }
        }

        private async Task<(T Result, ApiCallResponseError Errors)> PostAlborzServiceAsyncPrivate<T>(string uri, object model, Dictionary<string, string> headers = null, int timeout = 120000)
        {


            var DESC = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            byte[] dataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = timeout;

            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json";
            request.Method = "POST";

            if (headers != null)
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }

            using (Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var data = await reader.ReadToEndAsync();



                    return (Result: JsonConvert.DeserializeObject<T>(data), Errors: null);


                }

            }
            catch (WebException e)
            {
                Log.Error(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Name = "External Api Call Error",
                    Code = -10,
                    Object = uri + " | " + JsonConvert.SerializeObject(model) + JsonConvert.SerializeObject(e)
                }));

                return (Result: default(T), Errors: await handleError(e));


            }

        }


    }
}
