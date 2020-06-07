using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotORT.Simplex.SimplexEntity
{
    /// <summary>
    /// Vector Equation class. This class have a VectorBody = Constant format.
    /// Or VectorBody - Constant = 0.
    /// </summary>
    public class VectorEquation
    {
        public string Name { get; set; }
        public IDictionary<string, double> VectorBody { get; set; }
        public double Constant { get; set; }

        public VectorEquation() { }

        public VectorEquation(IDictionary<string, double> vector, double constant)
        {
            this.VectorBody = vector;
            this.Constant = constant;
        }
    }
}

