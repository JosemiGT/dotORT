using System;
using System.Collections.Generic;

namespace dotORT.Simplex.Entities
{
    public class ObjectiveFunction : VectorEquation
    {
        public bool isMaximice { get; set; }
        public ObjectiveFunction(IDictionary<string, double> vector, bool isMaximice) 
            : base(vector, 0)
        {
            this.isMaximice = isMaximice;
        }
        public ObjectiveFunction(VectorEquation vectorEquation, bool isMaximice)
            : base(vectorEquation.VectorBody, vectorEquation.Constant)
        {
            this.isMaximice = isMaximice;
        }
    }
}