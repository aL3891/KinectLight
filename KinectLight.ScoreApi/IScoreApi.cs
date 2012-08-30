using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KinectLight.ScoreApi
{
    public interface IScoreApi
    {
        /// <summary>
        /// Get single object or a list of objects
        /// </summary>
        /// <typeparam name="T">For single objects, e.g. GameDto. For a list; e.g. IList<GameDto></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(Uri relativeUri);

        /// <summary>
        /// Post stuff manually.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync<T>(Uri relativeUri, T dto);

        /// <summary>
        /// Convenience method. URI is predefined and hardcoded. :)
        /// </summary>
        /// <typeparam name="ScoreDto"></typeparam>
        /// <param name="score"></param>
        /// <param name="username"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostScoreAsync<ScoreDto>(ScoreDto score, string username, string gameName);

        /// <summary>
        /// Convenience method. URI is predefined and hardcoded. :)
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> CreateGameAsync(string gameName);

    }
}
