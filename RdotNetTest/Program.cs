using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;
namespace RdotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string result;
            string funcresult;
            string input;
            REngine engine;

            //init the R engine            
            REngine.SetEnvironmentVariables();
            engine = REngine.GetInstance();
            engine.Initialize();

            //prepare data
            List<int> size = new List<int>() { 29, 33, 51, 110, 357, 45, 338, 543, 132, 70, 103, 301, 146, 10, 56, 243, 238 };
            List<int> population = new List<int>() { 3162, 11142, 3834, 7305, 81890, 1339, 5414, 65697, 11280, 4589, 320, 60918, 480, 1806, 4267, 63228, 21327 };

            // define your function in R as multi-line string

            string myfuncstr = @"matrix_mult <- function(a,b){
	                                                            c = a %*% b;
	                                                            return(c);
	                                                          }";

            // embed your R function here

            Function matrix_mult = engine.Evaluate(myfuncstr).AsFunction();

            // create input matrices
            NumericMatrix a = engine.Evaluate(@"a <- matrix(c(1,4,2,5,3,6), nrow=2)").AsNumericMatrix();
            NumericMatrix b = engine.Evaluate(@"b <- matrix(c(1,4,2,5,3,6), nrow=3)").AsNumericMatrix();

            //calculate
            IntegerVector sizeVector = engine.CreateIntegerVector(size);
            engine.SetSymbol("size", sizeVector);

            IntegerVector populationVector = engine.CreateIntegerVector(population);
            engine.SetSymbol("population", populationVector);

            NumericVector resultVector = engine.Evaluate("cor(size, population)").AsNumeric();
            result = resultVector[0].ToString();

            // call your R function from c#
            NumericMatrix c = engine.Evaluate(@"c <- matrix_mult(a,b)").AsNumericMatrix();

            funcresult = c[0, 0].ToString() + ", " + c[0, 1].ToString() + ", " + c[1, 0].ToString() + ", " + c[1, 1].ToString();
            //clean up
            engine.Dispose();
            //output
            Console.WriteLine("");
            Console.WriteLine("Result: '{0}'", result);
            Console.WriteLine("Function Result: '{0}'", funcresult);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
