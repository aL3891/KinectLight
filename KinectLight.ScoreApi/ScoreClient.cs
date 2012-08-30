using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.Net.Http.Headers;

namespace KinectLight.ScoreApi
{
    public class ScoreClient : IScoreApi
    {
        private HttpClient client; // check lifetime of this
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(15);
        private Uri baseUri = new Uri("http://176.58.105.78:3000");

        /// <summary>
        /// Uses a default, predefined base URI.
        /// </summary>
        public ScoreClient()
            : this(null)
        { }

        /// <summary>
        /// Specify a base uri. No trailing slash.
        /// </summary>
        /// <param name="baseAddress"></param>
        public ScoreClient(Uri baseAddress)
        {
            if (baseAddress != null)
                this.baseUri = baseAddress;
            InitClient();
        }

        /// <summary>
        /// Initializes the client. Override this if you want to change default timeout, etc.
        /// </summary>
        public virtual void InitClient()
        {
            client = new HttpClient();
            client.Timeout = defaultTimeout;
        }

        /// <summary>
        /// Get single object or a list of objects
        /// </summary>
        /// <typeparam name="T">For single objects, e.g. GameDto. For a list; e.g. IList<GameDto></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(Uri relativeUri)
        {
            return GetJsonAsync(new Uri(this.baseUri, relativeUri)).ContinueWith(
                (getJsonTask) =>
                {
                    return JsonSerializer.DeserializeFromString<T>(getJsonTask.Result);
                });
        }


        /// <summary>
        /// Post stuff manually.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostAsync<T>(Uri relativeUri, T dto)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(JsonSerializer.SerializeToString<T>(dto));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return client.PostAsync(new Uri(this.baseUri, relativeUri), request.Content);
        }

        /// <summary>
        /// Convenience method. URI is predefined and hardcoded. :)
        /// </summary>
        /// <typeparam name="ScoreDto"></typeparam>
        /// <param name="score"></param>
        /// <param name="username"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostScoreAsync<ScoreDto>(ScoreDto score, string user, string gameName)
        {
            var scoreUri = string.Format("/games/{0}/score/{1}", gameName, user);
            Uri uri = new Uri(this.baseUri, scoreUri);
            return PostAsync<ScoreDto>(uri, score);
        }

        /// <summary>
        /// Convenience method. URI is predefined and hardcoded. :)
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> CreateGameAsync(string gameName)
        {
            Uri uri = new Uri(this.baseUri, "/games");
            return PostAsync<GameDto>(uri, new GameDto { name = gameName, _id = "" });
        }


        #region private methods
        
        /// <summary>
        /// Returns a task with the string result as a raw json string.
        /// </summary>
        /// <returns></returns>
        private Task<string> GetJsonAsync(Uri uri)
        {
            return client.GetAsync(uri).ContinueWith(
                (requestTask) =>
                {
                    HttpResponseMessage response = requestTask.Result;
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync();
                }).Unwrap(); // Solve the nested task and preserve exceptions
        }

        #endregion private methods
    }


}
