using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.ComparisonModels
{
    public class Faces
    {
        public decimal Confidence { get; set; }
        public Coodinates Coordinates { get; set; }
        public string Face_id { get; set; }
    }
}
