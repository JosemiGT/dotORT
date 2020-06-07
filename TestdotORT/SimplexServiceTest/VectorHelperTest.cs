using dotORT.Simplex.SimplexEntity;
using dotORT.Simplex.SimplexService;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestdotORT.SimplexServiceTest
{
    public class VectorHelperTest
    {
        [Fact]
        public void isEqualDimensionVectorsTest()
        {
            VectorHelper vectorHelper = new VectorHelper();

            List<VectorEquation> vectors = new List<VectorEquation>();
            VectorEquation eq1 = new VectorEquation(new Dictionary<string, double> {
                {"x1", 1},
                {"x2", 2}
            }, 0);
            VectorEquation eq2 = new VectorEquation(new Dictionary<string, double>
            {
                {"x1", 1},
                {"x2", 1}
            }, 0);

            vectors.Add(eq1);
            vectors.Add(eq2);

            Assert.True(vectorHelper.isEqualVariableVectors(vectors));
        }
    }
}
