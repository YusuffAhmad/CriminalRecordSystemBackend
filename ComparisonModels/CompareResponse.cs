using System;
using PrivateEye.ComparisonModels.New;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.ComparisonModels
{
    public class CompareResponse
    {
        public Results Result { get; set; }
        public Status Status { get; set; }
    }
}
