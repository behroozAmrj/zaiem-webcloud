using Cloud.IU.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IU.WEB.InfraSructure
{
    public interface ITask
    {
        TaskType _tasktype{get ; set;}
        void PerformTask(string Machine , User User);
    }
}
