using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VoronoiLib.Structures;
using VoronoiLib;

namespace VoronoiSpeedTest
{
    public class Program
    {
        private const int WIDTH = 10000;
        private const int HEIGHT = 10000;
        private const int MAX_N = 500000;
        private const int SAMPLES = 10;
        private const int INC = 20000;

        public static void Main(string[] args)
        {
            var r = new Random();
            var watch = new Stopwatch();
            var times = new long[MAX_N, SAMPLES];

    
            for (var point = 1; point * INC <= MAX_N; point++)
            {
                var numPoints = point * INC;
                Console.WriteLine($"Running for n = {numPoints}");
                for (var sample = 1; sample <= SAMPLES; sample++)
                {
                    Console.WriteLine($"\tRunning sample {sample}");
                    watch.Reset();
                    var points = GenPoints(numPoints, r);
                    watch.Start();
                    FortunesAlgorithm.Run(points, 0, 0, WIDTH, HEIGHT);
                    watch.Stop();
                    times[point - 1, sample - 1] = watch.ElapsedMilliseconds;
                }
            }

            var outFile = File.CreateText("timings.csv");
            var excelFile = File.CreateText("excelTimings.csv");
            outFile.Write("N, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10" + Environment.NewLine);
            excelFile.Write("N, T (ms)" + Environment.NewLine);
            for (var i = 1; i * INC <= MAX_N; i++)
            {
                var s = i * INC + ", ";
                for (var j = 0; j < SAMPLES - 1; j++)
                {
                    s += times[i - 1, j] + ", ";
                }
                s += times[i - 1, SAMPLES - 1] + Environment.NewLine;
                outFile.Write(s);

                for (var j = 1; j <= SAMPLES; j++)
                {
                    excelFile.Write(i*INC + ", " + times[i - 1, j - 1] + Environment.NewLine);
                }
            }
            outFile.Dispose();
            excelFile.Dispose();

        }


        //o(n) avg gen points
        private static List<FortuneSite> GenPoints(int n, Random r)
        {
            var points = new List<FortuneSite>();
            for (var i = 0; i < n; i++)
            {
                points.Add(new FortuneSite(r.NextDouble() * WIDTH, r.NextDouble()* HEIGHT));
            }

            //uniq the points
            points = UniquePoints(points);
            var moreNeeded = points.Count - n;
            if (moreNeeded > 0)
            {
                points.AddRange(GenPoints(moreNeeded, r));
                points = UniquePoints(points);
            }

            return points;
        }

        //o(n) uniq
        private static List<FortuneSite> UniquePoints(List<FortuneSite> points)
        {
            points.Sort((p1, p2) =>
            {
                if (p1.X.ApproxEqual(p2.X))
                {
                    if (p1.Y.ApproxEqual(p2.Y))
                        return 0;
                    if (p1.Y < p2.Y)
                        return -1;
                    return 1;
                }
                if (p1.X < p2.X)
                    return -1;
                return 1;
            });

            var unique = new List<FortuneSite>(points.Count / 2);
            var last = points.First();
            unique.Add(last);
            for (var index = 1; index < points.Count; index++)
            {
                var point = points[index];
                if (!last.X.ApproxEqual(point.X) ||
                    !last.Y.ApproxEqual(point.Y))
                {
                    unique.Add(point);
                    last = point;
                }
            }
            return unique;
        }
    }
}
