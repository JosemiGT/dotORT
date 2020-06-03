using System;
using System.Collections.Generic;

namespace dotORT.Simplex.SimplexEntity
{
    public class ObjectiveFunction : VectorEquation
    {
        public bool isMaximice { get; set; }
        public ObjectiveFunction(string name, IDictionary<string, double> vector, bool isMaximice) 
            : base(name, vector, 0)
        {
            this.isMaximice = isMaximice;
        }
    }
}