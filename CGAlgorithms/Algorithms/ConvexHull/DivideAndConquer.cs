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
        public static bool Compare(Point p1, Point p2)
        {
            return p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y);
        }
        public static void Merge_sort(ref List<Point> points)
        {
            if (points.Count <= 1)
                return;
            List<Point> lt = new List<Point>();
            List<Point> rt = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count / 2)
                    lt.Add(points[i]);
                else
                    rt.Add(points[i]);
            }
            Merge_sort(ref lt);
            Merge_sort(ref rt);
            points.Clear();
            int plt = 0, prt = 0;
            while (plt < lt.Count && prt < rt.Count)
            {
                if (Compare(lt[plt], rt[prt]))
                    points.Add(lt[plt++]);
                else
                    points.Add(rt[prt++]);
            }
            while (plt < lt.Count)
                points.Add(lt[plt++]);
            while (prt < rt.Count)
                points.Add(rt[prt++]);
        }
        public static void Get_tangent(ref int a, ref int b, List<Point> lt, List<Point> rt, bool up)
        {
            bool done = false;
            while (!done)
            {
                done = true;
                int a2, b2;
                while (true)
                {
                    if (up)
                        a2 = (a + 1) % lt.Count;
                    else
                        a2 = (a - 1 + lt.Count) % lt.Count;
                    Line l = new Line(rt[b], lt[a]);
                    if (HelperMethods.CheckTurn(l, lt[a2]) == (up ? Enums.TurnType.Left : Enums.TurnType.Right))
                        break;
                    a = a2;
                }
                while (true)
                {
                    if (up)
                        b2 = (b - 1 + rt.Count) % rt.Count;
                    else
                        b2 = (b + 1) % rt.Count;
                    Line l = new Line(lt[a], rt[b]);
                    if (HelperMethods.CheckTurn(l, rt[b2]) == (up ? Enums.TurnType.Right : Enums.TurnType.Left))
                        break;
                    b = b2;
                    done = false;
                }
            }
        }
        public static List<Point> Solve(List<Point> points)
        {
            if (points.Count < 6)
            {
                List<Point> outputPoints = new List<Point>();
                List<Line> lines = new List<Line>();
                List<Polygon> polygons = new List<Polygon>();
                Algorithm bruteforce = new JarvisMarch();
                bruteforce.Run(points, lines, polygons, ref outputPoints, ref lines, ref polygons);
                return outputPoints;
            }
            //preprocess
            List<Point> lt = new List<Point>(), rt = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count / 2)
                    lt.Add(points[i]);
                else
                    rt.Add(points[i]);
            }
            //devide
            lt = Solve(lt);
            rt = Solve(rt);
            // get rightmost on lt and leftmost on rt
            double mx = lt[0].X, mn = rt[0].X;
            int a = 0, b = 0;
            for (int i = 1; i < Math.Max(lt.Count, rt.Count); i++)
            {
                if (i < lt.Count && mx < lt[i].X)
                {
                    mx = lt[i].X; a = i;
                }
                if (i < rt.Count && mn > rt[i].X)
                {
                    mn = rt[i].X; b = i;
                }
            }
            //upper tanget
            int up_a = a, up_b = b;
            Get_tangent(ref up_a, ref up_b, lt, rt, true);
            //lower tanget
            int lw_a = a, lw_b = b;
            Get_tangent(ref lw_a, ref lw_b, lt, rt, false);
            //merge
            List<Point> hull = new List<Point>();
            while (true)
            {
                hull.Add(lt[up_a]);
                if (up_a == lw_a)
                    break;
                up_a = (up_a + 1) % lt.Count;
            }
            while (true)
            {
                hull.Add(rt[lw_b]);
                if (lw_b == up_b)
                    break;
                lw_b = (lw_b + 1) % rt.Count;
            }
            return hull;
        }
        public static void Remove_duplicates(ref List<Point> points)
        {
            int ptr = 1;
            while (ptr < points.Count)
            {
                if (points[ptr - 1].Equals(points[ptr]) || 
                    (ptr + 1 < points.Count && points[ptr].X - points[ptr - 1].X < 1e-6 && points[ptr + 1].X - points[ptr].X < 1e-6))
                    points.Remove(points[ptr]);
                else
                    ptr++;
            }
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Merge_sort(ref points);
            Remove_duplicates(ref points);
            outPoints = Solve(points);
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
