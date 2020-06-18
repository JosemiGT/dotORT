using dotORT.Simplex.Entities;
using dotORT.Simplex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace TestdotORT.SimplexServiceTest
{
    public class SimplexServiceTest
    {
        [Fact]
        public void StandarizeLessConstraintTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Constraint c1 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 18);
            c1.Name = "r1";

            Constraint c2 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 3}
            }, Simplex.Entities.EComparator.LessEqualThan, 42);
            c2.Name = "r2";

            Constraint c3 = new Constraint(new Dictionary<string, double> {
                {"x1", 3},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 24);
            c3.Name = "r3";

            List<Constraint> constraints = new List<Constraint> { c1, c2, c3 };

            List<Constraint> result = simplexService.StandarizeConstraint(constraints, out List<string> header).ToList();

            Assert.Equal(1, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["S2"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["S3"]);

            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(1, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["S2"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["S3"]);

            Assert.Equal(0, result.Where(c => c.Name.Equals("r3")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r3")).FirstOrDefault().VectorBody["S2"]);
            Assert.Equal(1, result.Where(c => c.Name.Equals("r3")).FirstOrDefault().VectorBody["S3"]);

            Assert.True(header.Contains("S1") && 
                        header.Contains("S2") &&
                        header.Contains("S3"));

        }

        [Fact]
        public void StandarizeGreatConstraintTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Constraint c1 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 2},
                {"x3", 3}
            }, Simplex.Entities.EComparator.GreaterEqualThan, 15);
            c1.Name = "r1";

            Constraint c2 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 3},
                {"x3", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 12);
            c2.Name = "r2";


            List<Constraint> constraints = new List<Constraint> { c1, c2};

            List<Constraint> result = simplexService.StandarizeConstraint(constraints, out List<string> header).ToList();

            Assert.Equal(0, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(-1, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["e1"]);
            Assert.Equal(1, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["A1"]);

            Assert.Equal(1, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["e1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["A1"]);

            Assert.True(header.Contains("S1") &&
                        header.Contains("e1") &&
                        header.Contains("A1"));
        }

        [Fact]
        public void StandarizeConstraintAndFOTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Constraint c1 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 2},
                {"x3", 3}
            }, Simplex.Entities.EComparator.GreaterEqualThan, 15);
            c1.Name = "r1";

            Constraint c2 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 3},
                {"x3", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 12);
            c2.Name = "r2";

            ObjectiveFunction fo = new ObjectiveFunction(new Dictionary<string, double>
            {
                {"x1", 3},
                {"x2", 2},
                {"x3", 4}
            },false);
            fo.Name = "FO";

            List<Constraint> constraints = new List<Constraint> { c1, c2 };

            List<Constraint> result = simplexService.StandarizeConstraint(constraints, out List<string> header).ToList();
            fo = simplexService.StandarizeObjectiveFunction(fo, header);

            Assert.Equal(0, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(-1, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["e1"]);
            Assert.Equal(1, result.Where(c => c.Name.Equals("r1")).FirstOrDefault().VectorBody["A1"]);

            Assert.Equal(1, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["S1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["e1"]);
            Assert.Equal(0, result.Where(c => c.Name.Equals("r2")).FirstOrDefault().VectorBody["A1"]);

            Assert.Equal(-3, fo.VectorBody["x1"]);
            Assert.Equal(-2, fo.VectorBody["x2"]);
            Assert.Equal(-4, fo.VectorBody["x3"]);
            Assert.Equal(0, fo.VectorBody["S1"]);
            Assert.Equal(0, fo.VectorBody["e1"]);
            Assert.Equal(1, fo.VectorBody["A1"]);

            Assert.True(header.Contains("S1") &&
                        header.Contains("e1") &&
                        header.Contains("A1"));
        }

        [Fact]
        public void PivotingTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Constraint c1 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 18);
            c1.Name = "r1";

            Constraint c2 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 3}
            }, Simplex.Entities.EComparator.LessEqualThan, 42);
            c2.Name = "r2";

            Constraint c3 = new Constraint(new Dictionary<string, double> {
                {"x1", 3},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 24);
            c3.Name = "r3";

            ObjectiveFunction fo = new ObjectiveFunction(new Dictionary<string, double>
            {
                {"x1", 3},
                {"x2", 2}
            }, true);
            fo.Name = "FO";

            List<Constraint> constraints = new List<Constraint> { c1, c2, c3 };

            SimplexTable table = new SimplexTable();
            table.StandardConstraint = simplexService.StandarizeConstraint(constraints, out List<string> header).ToList();
            table.ObjectiveFunction = simplexService.StandarizeObjectiveFunction(fo, header);

            bool isPovoting = simplexService.Pivoting(ref table, out KeyValuePair<string, double> refCol, out KeyValuePair<string, double> refRow);

            Assert.True(isPovoting);
            Assert.Equal("x1", refCol.Key);
            Assert.Equal(-3, refCol.Value);
            Assert.Equal("r3", refRow.Key);
            Assert.Equal(3, refRow.Value);
        }

        [Fact]
        public void ReduceRowsTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Constraint c1 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 18);
            c1.Name = "r1";

            Constraint c2 = new Constraint(new Dictionary<string, double> {
                {"x1", 2},
                {"x2", 3}
            }, Simplex.Entities.EComparator.LessEqualThan, 42);
            c2.Name = "r2";

            Constraint c3 = new Constraint(new Dictionary<string, double> {
                {"x1", 3},
                {"x2", 1}
            }, Simplex.Entities.EComparator.LessEqualThan, 24);
            c3.Name = "r3";

            ObjectiveFunction fo = new ObjectiveFunction(new Dictionary<string, double>
            {
                {"x1", 3},
                {"x2", 2}
            }, true);
            fo.Name = "FO";

            List<Constraint> constraints = new List<Constraint> { c1, c2, c3 };

            SimplexTable table = new SimplexTable();
            table.StandardConstraint = simplexService.StandarizeConstraint(constraints, out List<string> header).ToList();
            table.ObjectiveFunction = simplexService.StandarizeObjectiveFunction(fo, header);

            bool isPovoting = simplexService.Pivoting(ref table, out KeyValuePair<string, double> refCol, out KeyValuePair<string, double> refRow);
            bool isReduce = simplexService.ReduceRows(ref table, refCol, refRow);

            Assert.True(isPovoting);
            Assert.True(isReduce);
            Assert.Equal(24, table.ObjectiveFunction.Constant);
            Assert.Equal(-1, table.ObjectiveFunction.VectorBody.FirstOrDefault(v => v.Key.Equals("x2")).Value);
        }

        [Fact]
        public void CheckifEndTest()
        {
            PrimalSimplexService simplexService = new PrimalSimplexService(new VectorOperations(new VectorHelper()), new VectorHelper());

            Assert.False(simplexService.CheckEnd(null));

        }
    }
}
