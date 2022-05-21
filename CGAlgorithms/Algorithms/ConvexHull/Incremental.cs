using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
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
            if (points.Count >= 3)
            {
                int[] prv = new int[points.Count];
                int[] nxt = new int[points.Count];
                int ptr = 2;
                Line l = new Line(points[0], points[1]);
                if (HelperMethods.CheckTurn(l, points[2]) != Enums.TurnType.Right)
                {
                    nxt[0] = 1; prv[0] = 2;
                    nxt[1] = 2; prv[1] = 0;
                    nxt[2] = 0; prv[2] = 1;
                }
                else
                {
                    nxt[0] = 2; prv[0] = 1;
                    nxt[2] = 1; prv[2] = 0;
                    nxt[1] = 0; prv[1] = 2;
                }
                for (int i = 3; i < points.Count; i++)
                {
                    int up, lw;
                    while (true)
                    {
                        l = new Line(points[i], points[ptr]);
                        if (HelperMethods.CheckTurn(l, points[nxt[ptr]]) == Enums.TurnType.Left)
                        {
                            up = ptr;
                            break;
                        }
                        ptr = nxt[ptr];
                    }
                    ptr = i - 1;
                    while (true)
                    {
                        l = new Line(points[i], points[ptr]);
                        if (HelperMethods.CheckTurn(l, points[prv[ptr]]) == Enums.TurnType.Right)
                        {
                            lw = ptr;
                            break;
                        }
                        ptr = prv[ptr];
                    }
                    prv[up] = i;
                    nxt[lw] = i;
                    nxt[i] = up;
                    prv[i] = lw;
                    ptr = i;
                }
                do
                {
                    outPoints.Add(points[ptr]);
                    ptr = nxt[ptr];
                } while (points[ptr] != outPoints[0]);
            }
            else
                outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
