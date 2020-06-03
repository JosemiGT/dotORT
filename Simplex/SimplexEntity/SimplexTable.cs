using System.Collections.Generic;

namespace dotORT.Simplex.SimplexEntity
{
    public class SimplexTable
    {
        public ObjectiveFunction FuncionObjetivo { get; set; }
        public IEnumerable<VectorEquation> StandardConstraint { get; set; }
        public IDictionary<string, KeyValuePair<string,double>> Base { get; set; }
    }
    
}