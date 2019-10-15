
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeJanken
{
    public interface IJankenJudgeService
    {
        void NotifyDetect(Action<IList<JudgeResult>> callback);
    }
}
