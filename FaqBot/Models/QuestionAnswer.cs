using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaqBot.Models
{
    public class QuestionAnswer
    {
        public IEnumerable<string> Questions { get; set; }
        public string Answer { get; set; }
        public double Score { get; set; }
        public int Id { get; set; }
        public string Source { get; set; }
        public IEnumerable<FilterPair> MetaData { get; set; }
    }
}