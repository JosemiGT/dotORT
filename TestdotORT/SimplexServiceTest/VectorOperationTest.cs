using dotORT.Simplex.SimplexEntity;
using dotORT.Simplex.SimplexService;
using Simplex.SimplexEntity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestdotORT.SimplexServiceTest
{
    public class VectorOperationTest
    {
        [Fact]
        public void CalculateVectorsOperationsTest()
        {
            VectorHelper vectorHelper = new VectorHelper();
            VectorOperations vectorOperations = new VectorOperations(vectorHelper);

            List<VectorEquation> vectors = new List<VectorEquation>();
            VectorEquation eq1 = new VectorEquation(new Dictionary<string, double> {
                {"x1", 1},
                {"x2", 2}
            }, 3);
            VectorEquation eq2 = new VectorEquation(new Dictionary<string, double>
            {
                {"x1", 1},
                {"x2", 1}
            }, 4);

            VectorEquation resultAdd = vectorOperations.CalculateVectorsOperations(eq1, EOperation.Addition, eq2);
            Assert.Equal(resultAdd.Constant, eq1.Constant + eq2.Constant);

            VectorEquation resultSubt = vectorOperations.CalculateVectorsOperations(eq1, EOperation.Subtraction, eq2);
            Assert.Equal(resultSubt.Constant, eq1.Constant - eq2.Constant);

            VectorEquation resultMult = vectorOperations.CalculateVectorsOperations(eq1, EOperation.Multiplication, eq2);
            Assert.Equal(resultMult.Constant, eq1.Constant * eq2.Constant);

            VectorEquation resultDiv = vectorOperations.CalculateVectorsOperations(eq1, EOperation.Division, eq2);
            Assert.Equal(resultDiv.Constant, eq1.Constant / eq2.Constant);
        }

        [Fact]
        public void CalculateVectorConstantOperationsTest()
        {
            VectorHelper vectorHelper = new VectorHelper();
            VectorOperations vectorOperations = new VectorOperations(vectorHelper);

            List<VectorEquation> vectors = new List<VectorEquation>();
            VectorEquation eq1 = new VectorEquation(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 4}
            }, 3);

            double pivot = 5;

            VectorEquation resultAdd = vectorOperations.CalculateVectorConstantOperations(eq1, EOperation.Addition, pivot);
            Assert.Equal(resultAdd.Constant, eq1.Constant + pivot);

            VectorEquation resultSubt = vectorOperations.CalculateVectorConstantOperations(eq1, EOperation.Subtraction, pivot);
            Assert.Equal(resultSubt.Constant, eq1.Constant - pivot);

            VectorEquation resultMult = vectorOperations.CalculateVectorConstantOperations(eq1, EOperation.Multiplication, pivot);
            Assert.Equal(resultMult.Constant, eq1.Constant * pivot);

            VectorEquation resultDiv = vectorOperations.CalculateVectorConstantOperations(eq1, EOperation.Division, pivot);
            Assert.Equal(resultDiv.Constant, eq1.Constant / pivot);
        }
    }
}
