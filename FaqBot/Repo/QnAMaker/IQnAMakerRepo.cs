using FaqBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaqBot.Repo.QnAMaker
{
    public interface IQnAMakerRepo
    {
         Task<AskQuestionResponse> GenerateAnswer(AskQuestionRequest questionRequest, QnaServiceDetails serviceDetails);
    }
}
