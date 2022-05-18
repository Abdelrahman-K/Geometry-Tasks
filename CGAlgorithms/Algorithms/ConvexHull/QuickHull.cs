using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public List<Point> Get_left_set(Point lt, Point rt, List<Point> list)
        {
            List<Point> ret = new List<Point>();
            Line l = new Line(lt, rt);
            foreach (Point p in list)
            {
                if (HelperMethods.CheckTurn(l, p) == Enums.TurnType.Left)
                    ret.Add(p);
            }
            return ret;
        }
        public List<Point> Solve(Point lt, Point rt, List<Point> list)
        {
            List<Point> ret = new List<Point>();
            double mx = 0;
            Point mxH = lt;
            foreach (Point p in list)
            {
                double h = HelperMethods.CrossProduct(lt.Vector(rt), lt.Vector(p)) / HelperMethods.Dist(lt, rt); // can be done by rotating
                if (h - 1e-6 > mx)
                {
                    mx = h;
                    mxH = p;
                }
            }
            if (!mxH.Equals(lt))
            {
                ret.Add(mxH);
                ////Console.WriteLine(mxH.X + ", " + mxH.Y + "   " + depth);
                List<Point> ans = Solve(lt, mxH, Get_left_set(lt, mxH, list));
                foreach (Point p in ans)
                    ret.Add(p);
                ans = Solve(mxH, rt, Get_left_set(mxH, rt, list));
                foreach (Point p in ans)
                    ret.Add(p);
            }
            return ret;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Point mn = points[0], mx = points[0];
            foreach (Point p in points)
            {
                if (p.X < mn.X || (p.X == mn.X && p.Y < mn.Y)) mn = p;
                if (p.X > mx.X || (p.X == mx.X && p.Y > mx.X)) mx = p;
            } // if multiple x with the same y?
            outPoints.Add(mn);
            if (!mn.Equals(mx))
            {
                outPoints.Add(mx);
                List<Point> ans = Solve(mn, mx, Get_left_set(mn, mx, points)); //concatenate the 2 lists
                foreach (Point p in ans)
                    outPoints.Add(p);
                ans = Solve(mx, mn, Get_left_set(mx, mn, points));
                foreach (Point p in ans)
                    outPoints.Add(p);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
