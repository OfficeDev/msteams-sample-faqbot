
namespace FaqBot.Repo.QnAMaker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using FaqBot.Models;
    using Newtonsoft.Json;

    public class QnAMakerRepo : IQnAMakerRepo
    {
        public QnAMakerRepo()
        {

        }   
        
        /// <summary>
        /// Posts a question request to the QnA maker endpoint
        /// </summary>
        /// <param name="questionRequest">request payload to be sent to QnAService endpoint</param>
        /// <returns></returns>
        public async Task<AskQuestionResponse> GenerateAnswer(
            AskQuestionRequest questionRequest, 
            QnaServiceDetails serviceDetails)
        {
            var requestUri = new Uri(
                serviceDetails.BaseEndpoint, 
                $"knowledgebases/{serviceDetails.KnowledgeBaseId}/generateAnswer");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "EndpointKey", 
                    serviceDetails.EndpointKey);

                using (var response = await client.PostAsync(
                    requestUri,
                    new StringContent(
                        JsonConvert.SerializeObject(questionRequest),
                        Encoding.UTF8,
                        "application/json")))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AskQuestionResponse>(responseContent);
                };
                
            };
            
        }
        
    }
}