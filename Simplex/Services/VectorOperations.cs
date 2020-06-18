using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotORT.Simplex.Entities;

namespace dotORT.Simplex.Services
{
    public class VectorOperations
    {
        private readonly VectorHelper vectorHelper;

        public VectorOperations(VectorHelper vectorHelper)
        {
            this.vectorHelper = vectorHelper;
        }

        public VectorEquation CalculateVectorsOperations(VectorEquation vector1, EOperation operation, VectorEquation vector2)
        {            
            VectorEquation result = new VectorEquation(new Dictionary<string, double>(), 0);

            if(vectorHelper.isEqualVariableVectors(new List<VectorEquation> { vector1, vector2 })) 
            {
                List<string> vars = vector1.VectorBody.Keys.ToList();

                foreach (string var in vars)
                {
                    switch (operation)
                    {
                        case EOperation.Addition:
                            result.VectorBody.Add(var, vector1.VectorBody[var] + vector2.VectorBody[var]);
                            break;
                        case EOperation.Subtraction:
                            result.VectorBody.Add(var, vector1.VectorBody[var] - vector2.VectorBody[var]);
                            break;
                        case EOperation.Multiplication:
                            result.VectorBody.Add(var, vector1.VectorBody[var] * vector2.VectorBody[var]);
                            break;
                        case EOperation.Division:
                            result.VectorBody.Add(var, vector1.VectorBody[var] / vector2.VectorBody[var]);
                            break;
                    }
                }

                result.Constant = CalculateOperations(vector1.Constant, operation, vector2.Constant);

            }

            return result;
        }

        public VectorEquation CalculateVectorConstantOperations(VectorEquation vector, EOperation operation, double number)
        {
            VectorEquation result = new VectorEquation(new Dictionary<string, double>(), 0);

            if (vector != null && vector.VectorBody.Count > 0)
            {
                List<string> vars = vector.VectorBody.Keys.ToList();

                foreach (string var in vars)
                {
                    switch (operation)
                    {
                        case EOperation.Addition:
                            result.VectorBody.Add(var, vector.VectorBody[var] + number);
                            break;
                        case EOperation.Subtraction:
                            result.VectorBody.Add(var, vector.VectorBody[var] - number);
                            break;
                        case EOperation.Multiplication:
                            result.VectorBody.Add(var, vector.VectorBody[var] * number);
                            break;
                        case EOperation.Division:
                            result.VectorBody.Add(var, vector.VectorBody[var] / number);
                            break;
                    }
                }

                result.Constant = CalculateOperations(vector.Constant, operation, number);
            }

            return result;
        }

        public double CalculateOperations(double number1, EOperation operation, double number2)
        {
            switch (operation)
            {
                case EOperation.Addition:
                    return number1 + number2;
                case EOperation.Subtraction:
                    return number1 - number2;
                case EOperation.Multiplication:
                    return number1 * number2;
                case EOperation.Division:
                    return number1 / number2;
                default: 
                    return 0;
            }
        }
    }
}
