using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotORT.Simplex.SimplexEntity
{
    public class VectorEquation
    {
        public string Name { get; set; }
        public IDictionary<string, double> VectorBody { get; set; }
        public double Constant { get; set; }

        public VectorEquation(string name, IDictionary<string, double> vector, double constant)
        {
            this.Name = name;
            this.VectorBody = vector;
            this.Constant = constant;
        }
    }
}

