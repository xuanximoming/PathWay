using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public partial class CP_ClinicalPathNodes : List<CP_ClinicalPathNode>
    {
        public   CP_ClinicalPathNodes InitializePaths()
        {
            return this;
        }
    }
}
