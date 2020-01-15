using System;
using System.IO;
using System.Text;

namespace BioInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("SimMatrix.txt", Encoding.UTF8);
            var measureType = lines[0];

            if (measureType == "p")
                WithPenalty(measureType, lines);
            else
                WithoutPenalty(measureType, lines);
        }

        public static void WithPenalty(string measureType, string[] lines)
        {
            var u = lines[1];
            var w = lines[2];
            var matrix = new double[5, 5];
            for (var i = 3; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                for (var j = 0; j < 5; j++)
                    matrix[i - 3, j] = double.Parse(line[j]);
            }

            var (measure, d) = MatrixBuilder.GlobalWithPenalty(u, w, matrix);
            var optimalGlobalAlignment =GlobalAlignment.OptimalWithSimilarity(d, u, w);
            PrintResult("Global", "With penalty", u, optimalGlobalAlignment.uResult, w, optimalGlobalAlignment.wResult,
                    measure);
            
        }

        private static void WithoutPenalty(string measureType, string[] lines)
        {
            throw new NotImplementedException();
        }

        public static void PrintResult(string alignmentType,string measureType, string u, string uResult, string w, string wResult, double measure)
        {
            Console.WriteLine();
            Console.WriteLine(alignmentType);
            Console.WriteLine($"{measureType}: {measure}");
            Console.WriteLine($"u: {u} \nw: {w} \nuResult: {uResult} {uResult.Length} \nwResult: {wResult} {wResult.Length}");
        }

        static double[,] ConvertDistanceToSimilarity(double[,] distanceMatrix)
        {
            var similarityMatrix = new double[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];
            for (var i = 0; i < distanceMatrix.GetLength(0); i++)
            for (var j = 0; j < distanceMatrix.GetLength(1); j++)
                similarityMatrix[i, j] = 1 / (1 + distanceMatrix[i, j]);

            return similarityMatrix;
        }

        static void PrintMatrix(double[,] matrix)
        {
            Console.WriteLine("Converted Matrix");
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                    Console.Write($"{matrix[i, j]},");
                Console.WriteLine();
            }
        }
    }
}