using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public Point p0;
        public bool Compare(Point p1, Point p2)
        {
            double t = HelperMethods.CrossProduct(p0.Vector(p1), p0.Vector(p2));
            if (t > +1e-6) return true;
            if (t < -1e-6) return false;
            return p1.Y < p2.Y || (p1.Y == p2.Y && p1.X < p2.X);
        }
        public void Merge_sort(ref List<Point> points)
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
            while(plt < lt.Count && prt < rt.Count)
            {
                if (Compare(lt[plt], rt[prt]))
                    points.Add(lt[plt++]);
                else
                    points.Add(rt[prt++]);
            }
            while(plt < lt.Count)
                points.Add(lt[plt++]);
            while (prt < rt.Count)
                points.Add(rt[prt++]);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // getting bottom left corner
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[0].Y || (points[i].Y == points[0].Y && points[i].X < points[0].X))
                {
                    Point tmp = points[0];
                    points[0] = points[i];
                    points[i] = tmp;
                }
            }
            //for (int i = 1; i < points.Count; i++)
            //    points[i] = p0.Vector(points[i]);
            // sorting by angles with p0 ccw
            p0 = points[0];
            Merge_sort(ref points);
            // collinear points must be removed
            List<Point> v = new List<Point>();
            v.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                while (i + 1 < points.Count)
                {
                    Line l = new Line(points[0], points[i]);
                    if (HelperMethods.CheckTurn(p0.Vector(points[i]), p0.Vector(points[i + 1])) != Enums.TurnType.Colinear)
                        break;
                    i++;
                }
                v.Add(points[i]);
            }
            //for (int i = 1; i < points.Count; i++)
            //    points[i] = new Point(points[i].X + p0.X, points[i].Y + p0.Y);
            // graham scan
            Stack<Point> st = new Stack<Point>();
            if (v.Count > 3)
            {
                st.Push(v[0]);
                st.Push(v[1]);
                st.Push(v[2]);
                for (int i = 3; i < v.Count; i++)
                {
                    while (true)
                    {
                        Point cur = st.Peek();
                        st.Pop();
                        Point prev = st.Peek();
                        st.Push(cur);
                        //Line l = new Line(prev, cur);
                        //if (HelperMethods.CheckTurn(l, v[i]) == Enums.TurnType.Left)
                        if (HelperMethods.CheckTurn(prev.Vector(cur), prev.Vector(v[i])) == Enums.TurnType.Left)
                            break;
                        st.Pop();
                    }
                    st.Push(v[i]);
                }
                while (st.Count != 0)
                {
                    outPoints.Add(st.Peek());
                    st.Pop();
                }
            }
            else
                outPoints = v;
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
