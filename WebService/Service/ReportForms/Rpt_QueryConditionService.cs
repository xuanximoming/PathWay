using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Rpt_QueryCondition GetQueryCondition() 
        {
            Rpt_QueryCondition rpt = new Rpt_QueryCondition();
            return rpt;
        }
    }
}