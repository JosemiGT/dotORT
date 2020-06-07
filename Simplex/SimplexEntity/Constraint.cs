using System.Collections.Generic;

namespace dotORT.Simplex.SimplexEntity
{
    public class Constraint : VectorEquation
    {
        
        public string Operator { get; set; }

        public Constraint (IDictionary<string, double> _vectorBody, string _operator, double _constant )
            : base( _vectorBody, _constant)
            {
                this.Operator = _operator;
            }
    }
}