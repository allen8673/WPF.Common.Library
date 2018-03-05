using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WPF.Common.Library.Data;

namespace WPF.Common.Library.Api
{
    public class ApiCaller: PropertyBinder<ApiCaller>
    {
        public Uri ServerHost { get { return _PropBind.GetValue<Uri>(); } }
        public string CompId { get { return _PropBind.GetValue<string>(); } }

        /// <summary>
        /// 以HTTP GET上傳/取得資料
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static HttpResponseMessage Get(string apiUrl)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseCookies = false }))
            {
                // *** Create Cookies Ref:http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage ***
                client.BaseAddress = _Entity.ServerHost;
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("Cookie", $"CompId={_Entity.CompId};");

                // *** Authorization Setting ***
                return client.SendAsync(request).Result;
            }
        }

        /// <summary>
        /// 以HTPP POST上傳/取得資料
        /// </summary>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="content">Response Content</param>
        /// <returns></returns>
        public static HttpResponseMessage Post(string apiUrl, HttpContent content)
        {
            var cookieContainer = new CookieContainer();
            using (var client = new HttpClient(new HttpClientHandler()) { Timeout = TimeSpan.FromHours(1) })
            {
                client.BaseAddress = _Entity.ServerHost;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                cookieContainer.Add(client.BaseAddress, new Cookie("CompId", _Entity.CompId));
                var result = client.PostAsync(apiUrl, content);
                return result.Result;
            }
        }

        /// <summary>
        /// 以HTPP POST上傳/取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="data">上傳資料</param>
        /// <returns></returns>
        public static HttpResponseMessage PostAsJson<T>(string apiUrl, T data)
        {
            var cookieContainer = new CookieContainer();
            using (var client = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer }))
            {
                // *** Create Cookies Ref:http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage ***
                client.BaseAddress = _Entity.ServerHost;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cookieContainer.Add(client.BaseAddress, new Cookie("CompId", _Entity.CompId));

                return client.PostAsJsonAsync(apiUrl, data).Result;
            }
        }
    }
}
