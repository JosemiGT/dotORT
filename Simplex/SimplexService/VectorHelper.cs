using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotORT.Simplex.SimplexEntity;

namespace dotORT.Simplex.SimplexService
{
    public class VectorHelper
    {
        public bool isEqualDimensionVectors(IEnumerable<VectorEquation> equations)
        {
            bool isEqual = false;

            if(equations != null && equations.Count() > 0 
                && !equations.Any(eq => eq.VectorBody.Count() == 0))
            {
                isEqual = true;
                int size = equations.FirstOrDefault().VectorBody.Count();
                
                foreach(VectorEquation eq in equations)
                {
                    if (eq.VectorBody.Count() != size) return false;
                }
            }

            return isEqual;
        }

        public bool isEqualVariableVectors(IEnumerable<VectorEquation> equations)
        {
            bool isEqual = false;

            if (equations != null && equations.Count() > 0
            && !equations.Any(eq => eq.VectorBody.Count() == 0)
            && isEqualDimensionVectors(equations))
            {
                isEqual = true;
                List<string> variables = equations.FirstOrDefault().VectorBody.Keys.ToList();

                foreach(VectorEquation eq in equations)
                {
                    if (!isEqualStringEnumerable(variables, eq.VectorBody.Keys.ToList())) return false;
                }

            }

            return isEqual;
        }

        public bool isEqualStringEnumerable(IEnumerable<string> list1, IEnumerable<string> list2)
        {
            bool isEqual = false;

            if (list1 != null && list2 != null 
                && list1.Count() > 0 && list2.Count() > 0 
                && list1.Except(list2).Count() == 0
                && list2.Except(list1).Count() == 0) isEqual = true;

            return isEqual;
        }
    }
}
