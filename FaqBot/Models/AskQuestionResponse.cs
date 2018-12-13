using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaqBot.Models
{
    public class AskQuestionResponse
    {
        public IEnumerable<QuestionAnswer> Answers { get; set; }
    }
}