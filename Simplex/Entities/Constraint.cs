using Simplex.Entities;
using System.Collections.Generic;

namespace dotORT.Simplex.Entities
{
    public class Constraint : VectorEquation
    {
        
        public EComparator ComparatorOperator { get; set; }

        public Constraint (IDictionary<string, double> _vectorBody, EComparator _operator, double _constant )
            : base( _vectorBody, _constant)
            {
                this.ComparatorOperator = _operator;
            }
    }
}