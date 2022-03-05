using LearningContest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.HttpCall
{
    public interface IHttpCallService
    {
        Task<(T Result, ApiCallResponseError Errors)> GetAlborzServiceAsync<T>(string uri, Dictionary<string, string> headers, int timeout = 120000);
        Task<(T Result, ApiCallResponseError Errors)> PostAlborzServiceAsync<T>(string uri, object model, Dictionary<string, string> headers = null, int timeout = 120000);
    }

   



}
