using dotORT.Simplex.Entities;
using Simplex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotORT.Simplex.Services
{
    public class PrimalSimplexService
    {
        private readonly VectorOperations _vectorOperations;
        private readonly VectorHelper _vectorHelper;

        public PrimalSimplexService(VectorOperations vectorOperations, VectorHelper vectorHelper)
        {
            this._vectorOperations = vectorOperations;
            this._vectorHelper = vectorHelper;
        }

        #region Public Methods

        public IEnumerable<Constraint> StandarizeConstraint (IEnumerable<Constraint> constraints, out List<string> header)
        {
            List<Constraint> result = new List<Constraint>();
            header = new List<string>();

            if (_vectorHelper.isEqualVariableVectors(constraints))
            {
                int hVar = constraints.Where(c => (c.ComparatorOperator == EComparator.LessThan || c.ComparatorOperator == EComparator.LessEqualThan)).Count();
                int eVar = constraints.Where(c => (c.ComparatorOperator == EComparator.GreaterThan || c.ComparatorOperator == EComparator.GreaterEqualThan)).Count();
                int aVar = constraints.Where(c => c.ComparatorOperator == EComparator.EqualThan).Count();

                header = constraints.FirstOrDefault().VectorBody.Keys.ToList();

                for(int i = 1; i <= hVar; i++) header.Add(string.Format("S{0}", i.ToString()));

                for(int i = 1; i <= eVar; i++) header.Add(string.Format("e{0}", i.ToString()));

                for(int i = 1; i <= eVar + aVar; i++) header.Add(string.Format("A{0}", i.ToString()));

                int eqSCount = 1;
                int eqeCount = 1;

                foreach (Constraint constraint in constraints)
                {
                    Constraint constraintstandarize = new Constraint(constraint.VectorBody, EComparator.EqualThan, constraint.Constant);

                    if (!string.IsNullOrWhiteSpace(constraint.Name)) constraintstandarize.Name = constraint.Name;

                    List<string> headerVoid = header.Except(constraint.VectorBody.Keys).ToList();

                    if (constraint.ComparatorOperator == EComparator.LessThan || constraint.ComparatorOperator == EComparator.LessEqualThan)
                    {
                        foreach(string head in headerVoid)
                        {
                            if (head.Equals(string.Format("S{0}", eqSCount.ToString()))) constraintstandarize.VectorBody.Add(head, 1);
                            else constraintstandarize.VectorBody.Add(head, 0);
                        }

                        eqSCount++;
                    }
                    else if(constraint.ComparatorOperator == EComparator.GreaterThan || constraint.ComparatorOperator == EComparator.GreaterEqualThan)
                    {
                        foreach (string head in headerVoid)
                        {
                            if (head.Equals(string.Format("e{0}", eqeCount.ToString()))) constraintstandarize.VectorBody.Add(head, -1);
                            else if(head.Equals(string.Format("A{0}", eqeCount.ToString()))) constraintstandarize.VectorBody.Add(head, 1);
                            else constraintstandarize.VectorBody.Add(head, 0);
                        }
                        eqeCount++;
                    }

                    result.Add(constraintstandarize);
                                        
                }
            }

            return result;
        }

        public ObjectiveFunction StandarizeObjectiveFunction (ObjectiveFunction objectiveFunction, IEnumerable<string> header)
        {
            if(objectiveFunction != null && 
                objectiveFunction.VectorBody != null && 
                objectiveFunction.VectorBody.Count > 0)
            {
                List<string> voidHeader = header.Except(objectiveFunction.VectorBody.Keys).ToList();

                foreach (string head in header)
                {
                    if (objectiveFunction.VectorBody.Keys.Any(k => k.Equals(head))) objectiveFunction.VectorBody[head] = -objectiveFunction.VectorBody[head];
                    else if (head.Contains("A")) objectiveFunction.VectorBody.Add(head, 1);
                    else objectiveFunction.VectorBody.Add(head, 0);
                }
            }

            return objectiveFunction;
        }

        public bool Pivoting(ref SimplexTable table, out KeyValuePair<string, double> refCol, out KeyValuePair<string, double> refRow)
        {
            bool isCorrect = false;

            refCol = new KeyValuePair<string, double>("", double.MinValue);
            refRow = new KeyValuePair<string, double>();

            if (table != null)
            {
                if (table.ObjectiveFunction.isMaximice) refCol = new KeyValuePair<string, double>("", double.MaxValue);

                foreach (KeyValuePair<string, double> kv in table.ObjectiveFunction.VectorBody)
                {
                    if (table.ObjectiveFunction.isMaximice && kv.Value != 0 && kv.Value < refCol.Value) refCol = kv;
                    else if (!table.ObjectiveFunction.isMaximice && kv.Value != 0 && kv.Value > refCol.Value) refCol = kv;
                }

                refRow = GetRefRow(refCol, ref table);
                isCorrect = true;
            }

            return isCorrect;
        }

        public bool ReduceRows(ref SimplexTable table, KeyValuePair<string, double> refCol, KeyValuePair<string, double> refRow)
        {
            bool isCorrect = false;
            List<VectorEquation> result = new List<VectorEquation>();

            if(table != null && !string.IsNullOrEmpty(refCol.Key) && !string.IsNullOrEmpty(refCol.Key))
            {
                VectorEquation eqRef = _vectorOperations.CalculateVectorConstantOperations(
                                                            table.StandardConstraint.FirstOrDefault(c => c.Name.Equals(refRow.Key)), 
                                                            EOperation.Division, 
                                                            refRow.Value);

                foreach (VectorEquation equation in table.StandardConstraint)
                {
                    if (equation.Name.Equals(refRow.Key))
                    {
                        eqRef.Name = equation.Name;
                        result.Add(eqRef);
                    }
                    else
                    {
                        VectorEquation redEqu = _vectorOperations.CalculateVectorsOperations(
                            equation,
                            EOperation.Subtraction,
                            _vectorOperations.CalculateVectorConstantOperations(
                                        eqRef,
                                        EOperation.Multiplication,
                                        equation.VectorBody[refCol.Key])
                            );
                        redEqu.Name = equation.Name;
                        result.Add(redEqu);
                    }

                }

                double pivotFO = table.ObjectiveFunction.VectorBody.FirstOrDefault(f => f.Key == refCol.Key).Value;
                ObjectiveFunction ofResult = null;

                if (pivotFO != 0)
                {
                    VectorEquation redEqu = _vectorOperations.CalculateVectorsOperations(
                    table.ObjectiveFunction,
                    EOperation.Subtraction,
                    _vectorOperations.CalculateVectorConstantOperations(
                        eqRef,
                        EOperation.Multiplication,
                        pivotFO));

                    ofResult = new ObjectiveFunction(
                        redEqu,
                        table.ObjectiveFunction.isMaximice);
                }
                else
                {
                    ofResult = table.ObjectiveFunction;
                }

                table.StandardConstraint = result;
                table.ObjectiveFunction = ofResult;

                if (table.Base != null && table.Base.Count() > 0) table.Base = refreshBase(table.Base, refRow.Key, refCol.Key);

                isCorrect = true;
            }

            return isCorrect;
        }

        public bool CheckEnd(SimplexTable table)
        {
            bool isEnd = false;

            if (table != null && table.ObjectiveFunction != null)
            {
                foreach (string item in table.Base)
                {
                    if (table.ObjectiveFunction.isMaximice && !table.ObjectiveFunction.VectorBody.Any(v => v.Value < 0) || 
                        !table.ObjectiveFunction.isMaximice && !table.ObjectiveFunction.VectorBody.Any(v => v.Value > 0) ||
                        !table.StandardConstraint.Any(c => c.VectorBody.FirstOrDefault(v => v.Key.Equals(item)).Value < 0)) isEnd = true;
                }
            }
            return isEnd;
        }

        #endregion

        #region Private Methods

        private KeyValuePair<string, double> GetRefRow(KeyValuePair<string, double> refCol, ref SimplexTable tabla)
        {

            double valorCompareIteracion = new double();
            string restriccionS = string.Empty;
            double pivoteValor = double.NaN;

            valorCompareIteracion = double.MaxValue;

            foreach (VectorEquation eq in tabla.StandardConstraint)
            {
                double iteracionN = eq.VectorBody.Where(v => v.Key == refCol.Key).FirstOrDefault().Value;
                string iteracionS = !string.IsNullOrEmpty(eq.Name) ? eq.Name : string.Empty;
                if (iteracionN > 0 && (tabla.GetConstants().Where(v => v.Key == eq.Name).FirstOrDefault().Value / iteracionN) < valorCompareIteracion) 
                { 
                    pivoteValor = iteracionN; 
                    restriccionS = iteracionS; 
                    valorCompareIteracion = (tabla.GetConstants().Where(v => v.Key == eq.Name).FirstOrDefault().Value / iteracionN); 
                }

                if (tabla.IsSolution) tabla.IsSolution = (iteracionN > 0);
            }

            return new KeyValuePair<string, double>(restriccionS, pivoteValor);
        }

        private IEnumerable<string> refreshBase (IEnumerable<string> _base, string inVar, string outVar)
        {
            List<string> newBase = _base.ToList();
            newBase.Add(inVar);
            return newBase.Where(b => !string.Equals(b, outVar));
        }

        #endregion
    }
}
