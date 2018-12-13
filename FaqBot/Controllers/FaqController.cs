using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FaqBot.Models;
using FaqBot.Repo.QnAMaker;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;

namespace FaqBot.Controllers
{
    [BotAuthentication]
    [TenantFilter]
    public class FaqController : ApiController
    {
        private IConnectorClient connectorClient;
        private IQnAMakerRepo qNaRepo;
        public FaqController()
        {
            this.qNaRepo = new QnAMakerRepo();
        }

        /// <summary>
        /// Endpoint for bot request from Teams client
        /// </summary>
        /// <param name="activity">Request details</param>
        /// <returns>success</returns>
        /// POST: api/Faq
        public async Task<IHttpActionResult> PostAsync([FromBody] Activity activity)
        {
            var replyActivity = activity.CreateReply();

            try
            {
                // route activity text to QnA service
                var questionResponse = await qNaRepo.GenerateAnswer(
                    new AskQuestionRequest
                    {
                        Question = activity.Text,
                        Top = 1
                    },
                    getQnaServiceDetails());
            
                replyActivity.Text = questionResponse.Answers.FirstOrDefault().Answer;
            }
            catch
            {
                replyActivity.Text = "Uh oh. Brain freeze! Try asking me again :)";
            }

            this.connectorClient = new ConnectorClient(
                 new Uri(activity.ServiceUrl),
                 ConfigurationManager.AppSettings[MicrosoftAppCredentials.MicrosoftAppIdKey],
                 ConfigurationManager.AppSettings[MicrosoftAppCredentials.MicrosoftAppPasswordKey]);

            await connectorClient.Conversations.SendToConversationWithRetriesAsync(
                 replyActivity,
                activity.Conversation.Id);

            return Ok();
        }

        /// <summary>
        /// Returns service details for QnA service
        /// </summary>
        /// <returns></returns>
        private QnaServiceDetails getQnaServiceDetails()
        {
            string baseUri = ConfigurationManager.AppSettings["QnAEndpointBaseUri"];
            string knowledgeBaseId = ConfigurationManager.AppSettings["QnAKnowledgeBaseId"];
            string endpointKey = ConfigurationManager.AppSettings["QnAEndpointKey"];

            return new QnaServiceDetails(new Uri(baseUri), knowledgeBaseId, endpointKey);
        }
    }
}
