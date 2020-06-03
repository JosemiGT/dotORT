using System.Collections.Generic;

namespace dotORT.Simplex.SimplexEntity
{
    public class Constraint : VectorEquation
    {
        
        public string Operator { get; set; }

        public Constraint (string _name, IDictionary<string, double> _vectorBody, string _operator, double _constant )
            : base(_name, _vectorBody, _constant)
            {
                this.Operator = _operator;
            }
    }
}