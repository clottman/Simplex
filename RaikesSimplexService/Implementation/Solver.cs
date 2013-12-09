using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RaikesSimplexService.Contracts;
using RaikesSimplexService.DataModel;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RaikesSimplexService.InsertTeamNameHere
{

    public class Solver : ISolver
    {

        public DenseVector rhsValues { get; set; }

        public List<BasicVar> basics { get; set; }

        public int numConstraints { get; set; }

        public DenseMatrix xPrime { get; set; }

        public int numArtificial { get; set; }

        public List<int> artificialRows { get; set; }

        public Solution Solve(Model model)
        {
            DenseVector testVect = DenseVector.Create(10, delegate(int s) { return s; });

            DenseMatrix coefficients = createMatrix(model);

            //gets the coefficients from the goal
            DenseVector objFunCoeffs = new DenseVector(model.Goal.Coefficients);

            //adds zeroes for the surplus/slack/artificial variables
            DenseVector objFunValues = DenseVector.Create((coefficients.ColumnCount), delegate(int s) { return 0; });
            objFunValues.SetSubVector(0, objFunCoeffs.Count, objFunCoeffs);

            //flips the sign of everything
            if (model.GoalKind == GoalKind.Maximize)
            {
                objFunValues = DenseVector.Create((coefficients.ColumnCount), delegate(int s) { return -objFunValues[s]; });
            }

            //here so I don't have to pass the model in :)
            numConstraints = model.Constraints.Count;

            //Maybe I could be smarter about where I put this (at the end of createMatrix?)
            //take care of artificial variable stuff
            if (numArtificial > 0)
            {
                //adds the z row to the coefficients matrix
                //adds the column first
                coefficients = (DenseMatrix)coefficients.Append(DenseVector.Create((coefficients.RowCount), delegate(int s) { return 0; }).ToColumnMatrix());
                coefficients = (DenseMatrix)coefficients.InsertRow(coefficients.RowCount, DenseVector.Create(coefficients.ColumnCount, delegate(int s)
                {
                    if (s == coefficients.ColumnCount - 1)
                        return 1;
                    else
                        return objFunValues[s];
                }));


                //adds a 0 to match the z row
                rhsValues = DenseVector.Create(rhsValues.Count + 1, delegate(int s)
                {
                    if (s == rhsValues.Count)
                    {
                        return model.Goal.ConstantTerm;
                    }
                    else
                    {
                        return rhsValues[s];
                    }
                });

                //changes the objFunValues to be the w thingy
                objFunValues = DenseVector.Create(coefficients.ColumnCount, delegate(int s)
                {
                    double sum = 0;

                    //minus 1 because we also added a column for z
                    if (s < coefficients.ColumnCount - numArtificial - 1)
                    {
                        //calculate each w row entry by 
                        foreach (int index in artificialRows)
                        {
                            sum += coefficients[index, s];
                        }

                        return -sum;
                    }
                    else
                    {
                        return 0;
                    }
                });
                
                //z is now a basic variable??
                BasicVar zBase = new BasicVar(coefficients.RowCount - 1, coefficients.ColumnCount - 1);
                basics.Add(zBase);

                printMat(coefficients);

                //solves it for that
                coefficients = optimize(coefficients, objFunValues, true);

                printMat(coefficients);

                //removes z from basics
                var toRemove = (from aBasic in basics where aBasic.column == (coefficients.ColumnCount - 1) select aBasic).SingleOrDefault();
                basics.Remove(toRemove);

                //remakes the objective function
                objFunValues = (DenseVector)coefficients.SubMatrix(coefficients.RowCount - 1, 1, 0, coefficients.ColumnCount - 1 - numArtificial).Row(0);

                //chops off the z row and the artificial variables
                coefficients = (DenseMatrix)coefficients.SubMatrix(0, coefficients.RowCount - 1, 0, coefficients.ColumnCount - 1 - numArtificial);
            }

            printMat(coefficients);
            optimize(coefficients, objFunValues, false);

            /*
             * To find the solution:
             *      The first columns in the mattress (that aren't
             *      ssa variables) should be basic variables
             *      Find what the final value is by finding their indices
             *      in basics and then the value at that index in xprimes
             *      should be what you're looking for    I think
             *      At least it's somewhere in xprimes
             */
            double[] solution = new double[model.Goal.Coefficients.Length];
            double op = model.Goal.ConstantTerm;

            for (int i = 0; i < solution.Length; i++)
            {
                var row = (from aBasic in basics where aBasic.column == i select aBasic).SingleOrDefault();
                solution[i] = xPrime[basics.IndexOf(row), 0];
                op += solution[i] * model.Goal.Coefficients[i];
            }

            Solution sol = new Solution()
            {
                Decisions = solution,
                OptimalValue = op,
                AlternateSolutionsExist = false,
                Quality = SolutionQuality.Optimal
            };


            return sol;
        }

        private DenseMatrix optimize(DenseMatrix coefficients, DenseVector objFunValues, bool artifical)
        {
            //for calculations on the optimal solution row
            int cCounter,
                width = coefficients.ColumnCount;
            DenseVector cBVect = new DenseVector(basics.Count);

            //Sets up the b matrix
            DenseMatrix b = new DenseMatrix(basics.Count, 1);

            //basics : info about where the basic variables are
            // b : columns that are for basic variables

            //basics will have values greater than coefficients.ColumnCount - 1 if there are still artificial variables
            //or if Nathan is bad and didn't get rid of them correctly
            foreach (BasicVar aBasic in basics)
            {
                b = (DenseMatrix)b.Append(DenseVector.OfVector(coefficients.Column(aBasic.column)).ToColumnMatrix());
            }
            // removes the first column
            b = (DenseMatrix)b.SubMatrix(0, b.RowCount, 1, b.ColumnCount - 1);

            double[] cPrimes = new double[width];
            double[] rhsOverPPrime;
            DenseMatrix[] pPrimes = new DenseMatrix[width];
            DenseMatrix bInverse;

            int newEntering, exitingRow;

            // if all of the objective function values are positive, we already have an optimal solution 
            bool optimal = true;
            int objFunIterator = 0;
                while (optimal && objFunIterator < objFunValues.Count) {
                // as soon as we find one that's negative, we know we still need to optimize
                if (objFunValues[objFunIterator] < 0){
                    optimal = false;
                }
                objFunIterator++;
            }

            if (artifical)
            {
                rhsOverPPrime = new double[numConstraints + 1];
            }
            else
            {
                rhsOverPPrime = new double[numConstraints];
            }

            while (!optimal)
            {   
                //calculates the inverse of b for this iteration
                bInverse = (DenseMatrix)b.Inverse();

                //updates the C vector with the most recent basic variables
                cCounter = 0;
                foreach (BasicVar aBasic in basics)
                {
                    cBVect[cCounter++] = objFunValues.At(aBasic.column);
                }

                //calculates the pPrimes and cPrimes
                for (int i = 0; i < coefficients.ColumnCount; i++)
                {
                    var basicCols = from aBasic in basics select aBasic.column;
                    if (!basicCols.Contains(i))
                    {
                        pPrimes[i] = (DenseMatrix)bInverse.Multiply((DenseMatrix)coefficients.Column(i).ToColumnMatrix());

                        //c' = objFunVals - cB * P'n
                        //At(0) to turn it into a double
                        DenseVector pPrimeTimesCB = (DenseVector)(pPrimes[i].LeftMultiply(cBVect));
                        cPrimes[i] = objFunValues.At(i) - pPrimeTimesCB.At(0);
                    }
                    else
                    {
                        //I might have written stuff that makes this else block unnecessary, but idk yet
                        pPrimes[i] = null;
                        cPrimes[i] = Double.NaN;
                    }
                }

                //RHS'
                xPrime = (DenseMatrix)bInverse.Multiply((DenseMatrix)rhsValues.ToColumnMatrix());

                //Starts newEntering as the first nonbasic
                newEntering = -1;
                int iter = 0;
                while (newEntering == -1)
                {
                    if (!(from aBasic in basics select aBasic.column).Contains(iter))
                    {
                        newEntering = iter;
                    }

                    iter++;
                }

                //new entering becomes the smallest cPrime that corresponds to a non-basic value
                for (int i = 0; i < cPrimes.Length; i++)
                {
                    if (cPrimes[i] < cPrimes[newEntering] && !(from aBasic in basics select aBasic.column).Contains(i))
                    {
                        newEntering = i;
                    }
                }

                //if the smallest cPrime is >= 0, ie they are all positive
                if (cPrimes[newEntering] >= 0)
                {
                    optimal = true;
                }
                else
                {
                    for (int i = 0; i < xPrime.RowCount; i++ )
                    {
                        double[,] pPrime = pPrimes[newEntering].ToArray();
                        rhsOverPPrime[i] = xPrime.ToArray()[i, 0] / pPrime[i, 0];
                    }

                    //fix me to deal with if all these values are negative
                    exitingRow = -1;
                    iter = 0;
                    while(exitingRow == -1 && iter < xPrime.RowCount)
                    {
                        if (rhsOverPPrime[iter] > 0)
                        {
                            exitingRow = iter;
                        }
                        else
                        {
                            iter++;
                        }
                    }

                    //if iter == xPrime.RowCount now, then it's unbounded, I think

                    for (int i = 0; i < xPrime.RowCount; i++)
                    {                      
                        if (rhsOverPPrime[i] <= rhsOverPPrime[exitingRow] && rhsOverPPrime[i] > 0)
                        {
                            // if they're equal, prioritize the one that is artifical
                            if (rhsOverPPrime[i] == rhsOverPPrime[exitingRow] && artificialRows.Contains(exitingRow))
                            {
                                // exiting row stays the same
                            }
                            else
                            {
                                exitingRow = i;
                            }
                        }
                    }

                    //var toFix = (from aBasic in basics where aBasic.row == exitingRow select aBasic).SingleOrDefault();
                    var toFix = basics[exitingRow];
                    toFix.column = newEntering;

                    b.SetColumn(basics.IndexOf(toFix), coefficients.Column(newEntering));
                }
            }

            if(artifical)
            {
                //do the things that change coefficients
                for(int i = 0; i < pPrimes.Length; i++)
                {
                    if(pPrimes[i] != null)
                    {
                        coefficients.SetColumn(i, pPrimes[i].Column(0));
                    }
                }

                for(int i = 0; i < basics.Count; i++)
                {
                    coefficients.SetColumn(basics[i].column, DenseVector.Create(coefficients.RowCount, delegate(int s)
                    {
                        if (s == i)
                            return 1;
                        else
                            return 0;
                    }));
                }

                rhsValues = (DenseVector)xPrime.SubMatrix(0, xPrime.RowCount - 1, 0, 1).Column(0);
            }

            return coefficients;
        }

        public DenseMatrix createMatrix(Model model)
        {
            int numConstraints = model.Constraints.Count;
            int numDecisionVars = model.Goal.Coefficients.Length;
            int varCounter = numDecisionVars;
            //  matrix(rows, columns)
            DenseMatrix coefficients = new DenseMatrix(numConstraints, numDecisionVars);
            DenseMatrix artificialVars = new DenseMatrix(numConstraints, 1);
            var constraintCounter = 0;
            this.rhsValues = new DenseVector(numConstraints);
            this.basics = new List<BasicVar>();
            this.artificialRows = new List<int>();
            foreach (var constraint in model.Constraints)
            {
                // if the constraint RHS is negative, invert the coefficients and flip the inequality sign
                if (constraint.Value < 0)
                {
                    for (int i = 0; i < constraint.Coefficients.Length; i++)
                    {
                        constraint.Coefficients[i] = constraint.Coefficients[i] * -1;
                    }
                    if (constraint.Relationship == Relationship.LessThanOrEquals)
                    {
                        constraint.Relationship = Relationship.GreaterThanOrEquals;
                    }
                    else if (constraint.Relationship == Relationship.GreaterThanOrEquals)
                    {
                        constraint.Relationship = Relationship.LessThanOrEquals;
                    }
                    // also flip the rhs value which we already put in the array for the simplex setup
                    constraint.Value = constraint.Value * -1;
                }

                rhsValues[constraintCounter] = constraint.Value;


                coefficients.SetRow(constraintCounter, 0, constraint.Coefficients.Length, new DenseVector(constraint.Coefficients));
                // if it's a less than, add a slack column to the coefs matrix
                if (constraint.Relationship == Relationship.LessThanOrEquals)
                {
                    DenseVector slack = DenseVector.Create(model.Constraints.Count, delegate(int s) { return 0; });
                    slack.At(constraintCounter, 1);
                    coefficients = (DenseMatrix)coefficients.Append(slack.ToColumnMatrix());

                    this.basics.Add(new BasicVar(constraintCounter, varCounter));
                }
                else
                {
                    // Need to add an artificial variable for >= and = constraints

                    DenseVector surplus = DenseVector.Create(model.Constraints.Count, delegate(int s) { return 0; });
                    surplus.At(constraintCounter, -1);
                    coefficients = (DenseMatrix)coefficients.Append(surplus.ToColumnMatrix());

                    DenseVector artificial = DenseVector.Create(model.Constraints.Count, delegate(int s) { return 0; });
                    artificial.At(constraintCounter, 1);
                    artificialVars = (DenseMatrix)artificialVars.Append(artificial.ToColumnMatrix());

                    // Keeps track of the rows with artificial variable, for setting w
                    artificialRows.Add(constraintCounter);
                }
                varCounter++;
                constraintCounter++;
            }



            // put the constraints and stuff into the matrix
            if (artificialVars.ColumnCount > 1)
            {
                artificialVars = (DenseMatrix)artificialVars.SubMatrix(0, artificialVars.RowCount, 1, artificialVars.ColumnCount - 1);

                for (int i = coefficients.ColumnCount; i < coefficients.ColumnCount + artificialVars.ColumnCount; i++)
                {
                    this.basics.Add(new BasicVar(artificialRows[i - coefficients.ColumnCount], i));
                }

                coefficients = (DenseMatrix)coefficients.Append(artificialVars);

                numArtificial = artificialVars.ColumnCount;
            }
            else
            {
                numArtificial = 0;
            }

            return coefficients;
        }

        public void printMat(DenseMatrix mattress)
        {

            for (int i = 0; i < mattress.RowCount; i++)
            {
                for (int j = 0; j < mattress.ColumnCount; j++)
                {
                    System.Diagnostics.Debug.Write(mattress[i, j] + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
    }
}
