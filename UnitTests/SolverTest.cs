using RaikesSimplexService.InsertTeamNameHere;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using RaikesSimplexService.DataModel;


namespace UnitTests
{


    /// <summary>
    ///This is a test class for SolverTest and is intended
    ///to contain all SolverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SolverTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void TodaysClass()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };


            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 1, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 6
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void Simple2PhaseFromPaper()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { -1, 2 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = -1
            };


            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 3, 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 5
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void ExampleSolveTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 8, 12 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 24
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 12, 12 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 36
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 0.2, 0.3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 3, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 0.6
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }




        //http://optlab.mcmaster.ca/~feng/4O03/LP.Degeneracy.pdf
        //degeneracy
        [TestMethod()]
        public void DegeneracyTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[3] { 0, 1, 1 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 2
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }



        //two phase  but not really, Nathan's bad at naming
        [TestMethod()]
        public void RevisedClassTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 35
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 38
            };


            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 50
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 350, 450 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 12, 13 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 10050 // 12*350 + 13*450
            };

            var actual = target.Solve(model);

            #endregion
            ////Act

            ////Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // many variables and constraints
        [TestMethod()]
        public void nurseTest()
        {

            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 3
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc6 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 3
            };

            var lc7 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc8 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc9 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc10 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc11 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var lc12 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc13 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 8
            };

            var lc14 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 9
            };

            var lc15 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 7
            };

            var lc16 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 11
            };

            var lc17 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc18 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 2
            };

            var lc19 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 9
            };

            var lc20 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc21 = new LinearConstraint()
            {
                Coefficients = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc22 = new LinearConstraint()
            {
                Coefficients = new double[21] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 60
            };


            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5, lc6, lc7, lc8, lc9, lc10, lc11, lc12, lc13, lc14, lc15, lc16, lc17, lc18, lc19, lc20, lc21, lc22 };

            var goal = new Goal()
            {
                Coefficients = new double[21] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[21] { 3, 1, 0, 0, 2, 0, 0, 2, 4.5, 0, 0, 2.5, 0, 2.5, 4, 6, 0, 0, 5, 0, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 32.5
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // all constraints are equal
        [TestMethod()]
        public void equalSignConstraintsTest()
        {

            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[10] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.Equals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[10] { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.Equals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[10] { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
                Relationship = Relationship.Equals,
                Value = 1
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[10] { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
                Relationship = Relationship.Equals,
                Value = 1
            };

            var lc5 = new LinearConstraint()
            {
                Coefficients = new double[10] { 3, 0, 6, 0, 5, 0, 7, 0, 1, 0 },
                Relationship = Relationship.Equals,
                Value = 13
            };

            var lc6 = new LinearConstraint()
            {
                Coefficients = new double[10] { 0, 2, 0, 4, 0, 10, 0, 4, 0, 1 },
                Relationship = Relationship.Equals,
                Value = 10
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4, lc5, lc6 };

            var goal = new Goal()
            {
                Coefficients = new double[10] { 2, 11, 7, 7, 20, 2, 5, 5, 0, 0 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[10] { 1, 0, 1, 0, .24, .76, .4, .6, 0, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 20.32
            };

            /*
             * If we are rounding to whole numbers:
             * 
             * var expected = new Solution()
             *{
             *   Decisions = new double[10] { 1, 0, 0, 1, 1, 0, 0, 1, 5, 2 },
             *   Quality = SolutionQuality.Optimal,
             *   AlternateSolutionsExist = false,
             *   OptimalValue = 34
             *};
             * 
             * */

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // There is one =, one >=, one <= constraints
        [TestMethod()]
        public void AllRelationshipTypesTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 3, 1 },
                Relationship = Relationship.Equals,
                Value = 3
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 4, 3 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 3
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 4, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 0.6, 1.2 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 3.6
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // There is a constant term in the objective function
        [TestMethod()]
        public void constantInGoalTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 10, 5 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 50
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 6, 6 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 36
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 4.5, 18 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 81
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 9, 7 },
                ConstantTerm = 10
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 2 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 60
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        [TestMethod()]
        public void NegativeRHSTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[4] { -1, 6, -1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = -3
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[4] { 0, 7, 0, 2 },
                Relationship = Relationship.Equals,
                Value = 5
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[4] { 1, 1, 1, 0 },
                Relationship = Relationship.Equals,
                Value = 1
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[4] { 0, 0, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[4] { 3, -1, 0, 0 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[4] { 0, .71, .29, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = -0.71
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);

        }


        [TestMethod()]
        public void MaxFrom2PhaseSlidesTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                // No decisions
                Quality = SolutionQuality.Unbounded,
                AlternateSolutionsExist = false,
            };

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        [TestMethod()]
        public void MinFrom2PhaseSlidesTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 1
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 3 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 6, 3 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { .67, .33 },
                OptimalValue = 5.01,
                AlternateSolutionsExist = false,
                Quality = SolutionQuality.Optimal
            };

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        [TestMethod()]
        public void AlternateSolutionsExistTest()
        {
            // From HW 4, Question 3.

            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 40
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 2, 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 2, 3, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[3] { 10, 10, 20 },
                OptimalValue = 70,
                AlternateSolutionsExist = true,
                Quality = SolutionQuality.Optimal
            };

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        [TestMethod()]
        public void ScrewMixTest()
        {
            // From end of LP Intro and Graphical PPT

            #region Arrange
            var target = new Solver();

            // decision variables go in order of:
            // MixAType1, MixAType2, MixAType3, MixAType4, MixBType1 etc.

            var a = new LinearConstraint()
            {
                Coefficients = new double[12] { -.6, .4, .4, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var b = new LinearConstraint()
            {
                Coefficients = new double[12] { .2, -.8, .2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var c = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, -.8, .2, .2, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 0
            };

            var d = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, .4, -.6, .4, 0, 0, 0, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var e = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, -.5, .5, .5, 0, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                //flipped this one, and I think it should be like this
                Value = 0
            };

            var f = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 0, 0, 0, 0, .1, -.9, .1, 0, 0, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 0
            };

            var g = new LinearConstraint()
            {
                Coefficients = new double[12] { 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var h = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var i = new LinearConstraint()
            {
                Coefficients = new double[12] { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 1000
            };

            var constraints = new List<LinearConstraint>() { a, b, c, d, e, f, g, h, i };

            var goal = new Goal()
            {
                Coefficients = new double[12] { 10, 30, 42, -25, -5, 7, -15, 5, 17, -30, -10, 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[12] { 1000, 400, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                OptimalValue = 47200,
                AlternateSolutionsExist = true,
                Quality = SolutionQuality.Optimal
            };

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        [TestMethod()]
        public void InfeasibleTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, -2 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 4
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Quality = SolutionQuality.Infeasible
            };

            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            Assert.AreEqual(expected.Quality, actual.Quality);
        }


        // from first simplex slides
        [TestMethod()]
        public void SinglePhaseTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 10, 5 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 50
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 6, 6 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 36
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 4.5, 18 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 81
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 9, 7 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 2 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 50
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // from internets
        [TestMethod()]
        public void OnePhaseTest()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 8
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 2 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 0, 5 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 10
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

        // from internet
        [TestMethod()]
        public void SimpleTwoPhase()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 4
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 2, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 6
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 2, 2 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 4
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        // from the internets
        [TestMethod()]
        public void AnotherTwoPhase()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[3] { 1, 1, 1 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 40
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[3] { 2, 1, -1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[3] { 0, -1, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 10
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3 };

            var goal = new Goal()
            {
                Coefficients = new double[3] { 2, 3, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[3] { 10, 10, 20 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 70
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        // from michael. Alternate solutions
        /// <summary>
        /// #2 from http://www.ms.uky.edu/~rwalker/Class%20Work%20Solutions/class%20work%208%20solutions.pdf
        ///</summary>
        [TestMethod()]
        public void MsUKY2()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 6
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 3, 2 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 12
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { -2, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Minimize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 4, 0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = -8
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }


        // from michael. changes "minimize" to maximize
        /// <summary>
        ///First Example from LPSimplexMethod.pptx.pdf
        ///Tests ability of solver to solve a "Minimize Goal" problem
        ///Solver must convert the Minimize goal to a Maximize goal, then can proceed normally
        ///</summary>
        [TestMethod()]
        public void Maximize()
        {
            #region Arrange
            var target = new Solver();

            var lc1 = new LinearConstraint()
            {
                Coefficients = new double[2] { 50, 24 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2400
            };

            var lc2 = new LinearConstraint()
            {
                Coefficients = new double[2] { 30, 33 },
                Relationship = Relationship.LessThanOrEquals,
                Value = 2100
            };

            var lc3 = new LinearConstraint()
            {
                Coefficients = new double[2] { 1, 0 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 20
            };

            var lc4 = new LinearConstraint()
            {
                Coefficients = new double[2] { 0, 1 },
                Relationship = Relationship.GreaterThanOrEquals,
                Value = 5
            };

            var constraints = new List<LinearConstraint>() { lc1, lc2, lc3, lc4 };

            var goal = new Goal()
            {
                Coefficients = new double[2] { 1, 1 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[2] { 960.0 / 31.0, 1100.0 / 31.0 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                //OptimalValue = 2160.0 / 31.0
                OptimalValue = 66.451612903225808
            };
            #endregion

            //Act
            var actual = target.Solve(model);

            //Assert
            CollectionAssert.AreEqual(expected.Decisions, actual.Decisions);
            Assert.AreEqual(expected.OptimalValue, actual.OptimalValue);
            Assert.AreEqual(expected.Quality, actual.Quality);
            Assert.AreEqual(expected.AlternateSolutionsExist, actual.AlternateSolutionsExist);
        }

    }

}
