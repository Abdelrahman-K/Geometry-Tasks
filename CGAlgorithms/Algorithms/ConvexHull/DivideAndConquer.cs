using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public static void upperTangent(ref int a, ref int b, List<Point> left, List<Point> right)
        {
            bool cont = false;
            while (!cont)
            {
                cont = true;
                int a2, b2;
                while (true)
                {
                    a2 = (a + 1) % left.Count;
                    if (HelperMethods.CheckTurn(new Line(right[b], left[a]), left[a2]) == Enums.TurnType.Left) break;
                    a = a2;
                }
                while (true)
                {
                    b2 = (b + right.Count - 1) % right.Count;
                    if (HelperMethods.CheckTurn(new Line(left[a], right[b]), right[b2]) == Enums.TurnType.Right) break;
                    b = b2;
                    cont = false;
                }
            }
        }
        public static void lowerTangent(ref int a, ref int b, List<Point> left, List<Point> right)
        {
            bool cont = false;
            while (!cont)
            {
                cont = true;
                int a2, b2;
                while (true)
                {
                    a2 = (a + left.Count - 1) % left.Count;
                    if (HelperMethods.CheckTurn(new Line(right[b], left[a]), left[a2]) == Enums.TurnType.Right) break;
                    a = a2;
                }
                while (true)
                {
                    b2 = (b + 1) % right.Count;
                    if (HelperMethods.CheckTurn(new Line(left[a], right[b]), right[b2]) == Enums.TurnType.Left) break;
                    b = b2;
                    cont = false;
                }
            }
        }
        public static List<Point> dividAndConquerRecursive(List<Point> points)
        {
            if (points.Count < 6)
            {
                List<Point> outputPoints = new List<Point>();
                List<Line> lines = new List<Line>();
                List<Polygon> polygons = new List<Polygon>();
                Algorithm convexHullLastPoints = new JarvisMarch();
                convexHullLastPoints.Run(points, lines, polygons, ref outputPoints, ref lines, ref polygons);
                return outputPoints;
            }
            List<Point> left = new List<Point>(), right = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count / 2) left.Add(points[i]);
                else right.Add(points[i]);
            }
            left = dividAndConquerRecursive(left);
            right = dividAndConquerRecursive(right);
            double mxLeft = left[0].X, mnRight = right[0].X;
            int idx1 = 0, idx2 = 0, N = Math.Max(left.Count, right.Count);
            for (int i = 1; i < N; i++)
            {
                if (i < right.Count && mnRight > right[i].X)
                {
                    mnRight = right[i].X;
                    idx2 = i;
                }
                if (i < left.Count && mxLeft < left[i].X)
                {
                    mxLeft = left[i].X; 
                    idx1 = i;
                }
            }
            int upper1 = idx1, upper2 = idx2, lower1 = idx1, lower2 = idx2;
            upperTangent(ref upper1, ref upper2, left, right);
            lowerTangent(ref lower1, ref lower2, left, right);
            List<Point> convexHull = new List<Point>();
            while (true)
            {
                convexHull.Add(left[upper1]);
                if (upper1 == lower1) break;
                upper1 = (upper1 + 1) % left.Count;
            }
            while (true)
            {
                convexHull.Add(right[lower2]);
                if (lower2 == upper2) break;
                lower2 = (lower2 + 1) % right.Count;
            }
            return convexHull;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 0; i < points.Count; i++) points[i].Flag = true;
            points.Sort();
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i - 1].Equals(points[i]) ||
                    (i + 1 < points.Count && points[i].X - points[i - 1].X < 1e-6 && points[i + 1].X - points[i].X < 1e-6))
                {
                    points.RemoveAt(i);
                    i --;
                }
            }
            outPoints = dividAndConquerRecursive(points);
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
    }
}
