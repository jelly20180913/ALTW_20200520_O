﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using WebApi.Models.CustomModel;
using WebApi.Models.JWT;
using WebApi;
using WebApi.DataModel.CustomModel.SAP;
namespace TestWebApi.Common
{
    public class CallWebAPI
    {
        public static string _Token = "";
        public static string Login(string jsonData, string uri)
        {
            string result = "";
            using (WebClient webClient = new WebClient())
            {
                // 指定 WebClient 編碼
                webClient.Encoding = Encoding.UTF8;
                // 指定 WebClient 的 Content-Type header
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                // 指定 WebClient 的 authorization header
                //webClient.Headers.Add("authorization", "token {apitoken}"); 
                // 執行 post 動作
                result = webClient.UploadString(uri, jsonData);
                ApiResultEntity _ApiResult = JsonConvert.DeserializeObject<ApiResultEntity>(result);
                ReturnToken _ReturnToken = JsonConvert.DeserializeObject<ReturnToken>(_ApiResult.Data.ToString());
                _Token = _ReturnToken.token;
            }
            return _Token;
        }
        public static ApiResultEntity Post(string jsonData, string uri)
        {
            string result = "";
            using (WebClient webClient = new WebClient())
            {
                // 指定 WebClient 編碼
                webClient.Encoding = Encoding.UTF8;
                // 指定 WebClient 的 Content-Type header
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                // 指定 WebClient 的 authorization header
                webClient.Headers.Add("authorization", "Bearer "+_Token);  
                // 執行 post 動作
                result = webClient.UploadString(uri, jsonData); 
            }
            ApiResultEntity _ApiResult = JsonConvert.DeserializeObject<ApiResultEntity>(result);
            return _ApiResult;
        }
        public static ApiResultEntity Get( string uri, string token)
        {
            string result = "";
            using (WebClient webClient = new WebClient())
            {
                // 指定 WebClient 編碼
                webClient.Encoding = Encoding.UTF8;
                // 指定 WebClient 的 Content-Type header
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                // 指定 WebClient 的 authorization header
                webClient.Headers.Add("authorization", "Bearer " + token);
                // 執行 gett 動作
                result = webClient.DownloadString(uri);
            }
            ApiResultEntity _ApiResult = JsonConvert.DeserializeObject<ApiResultEntity>(result);
            return _ApiResult;
        }
    }
}
