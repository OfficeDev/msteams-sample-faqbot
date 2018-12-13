using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaqBot.Models
{
    public class QnaServiceDetails
    {
        public Uri BaseEndpoint { get; private set; }
        public string EndpointKey { get; private set; }
        public string KnowledgeBaseId { get; private set; }
        public QnaServiceDetails(
            Uri baseEndpoint, 
            string knowledgeBaseId, 
            string endpointKey)
        {
            this.BaseEndpoint = baseEndpoint;
            this.KnowledgeBaseId = knowledgeBaseId;
            this.EndpointKey = endpointKey;
        }
    }
}