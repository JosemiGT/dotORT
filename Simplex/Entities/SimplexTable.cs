using System.Collections.Generic;
using System.Linq;

namespace dotORT.Simplex.Entities
{
    public class SimplexTable
    {
        public ObjectiveFunction ObjectiveFunction { get; set; }
        public IEnumerable<VectorEquation> StandardConstraint { get; set; }
        //public IDictionary<string, KeyValuePair<string,double>> Base { get; set; }
        public IEnumerable<string> Base { get; set; }
        public bool IsSolution { get; set; }
        public IDictionary<string, double> GetConstants()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            if(this.StandardConstraint != null && this.StandardConstraint.Count() > 0)
            {
                if(!this.StandardConstraint.Any(c => !string.IsNullOrWhiteSpace(c.Name))) foreach (VectorEquation eq in this.StandardConstraint) result.Add(eq.Name, eq.Constant);
                else
                {
                    int count = 1;

                    foreach(VectorEquation eq in this.StandardConstraint)
                    {
                        if(!string.IsNullOrWhiteSpace(eq.Name)) result.Add(eq.Name, eq.Constant);
                        else result.Add(string.Format("EQ{0}",count.ToString()), eq.Constant);

                        count++;
                    }
                }
            }

            return result;
        }

    }
    
}