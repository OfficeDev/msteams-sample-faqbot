using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaqBot.Models
{
    public class AskQuestionRequest
    {
        public string Question { get; set; }
        public int Top { get; set; }
        public ICollection<FilterPair> StrictFilters {get;set;}
        public string UserId { get; set; }

    }
}