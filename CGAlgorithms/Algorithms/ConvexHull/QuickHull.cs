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
        public List<Point> left_points(Point lt, Point rt, List<Point> list)
        {
            List<Point> ret = new List<Point>();
            for (int i = 0; i < list.Count; i++)
                if (HelperMethods.CheckTurn(new Line(lt, rt), list[i]) == Enums.TurnType.Left)
                    ret.Add(list[i]);
            return ret;
        }
        public List<Point> quickHullRecursive(Point leftP, Point rightP, List<Point> points)
        {
            double mxHeight = 0;
            Point mxHeightPoint = leftP;
            for (int i = 0; i < points.Count; i++)
            {
                double curHeight = HelperMethods.CrossProduct(leftP.Vector(rightP), leftP.Vector(points[i]) / HelperMethods.Distance(leftP, rightP));
                if (curHeight > mxHeight)
                {
                    mxHeightPoint = points[i];
                    mxHeight = curHeight;
                }
            }
            List<Point> lft1 = left_points(leftP, mxHeightPoint, points);
            List<Point> lft2 = left_points(mxHeightPoint, rightP, points);
            List<Point> ans1 = quickHullRecursive(leftP, mxHeightPoint, lft1);
            List<Point> ans2 = quickHullRecursive(mxHeightPoint, rightP, lft2);
            List<Point> ret = list_concat(mxHeightPoint, ans1, ans2);
            return ret;
        }
        public List<Point> list_concat(Point mx, List<Point> p1, List<Point> p2)
        {
            List<Point> ret = new List<Point>();
            ret.Add(mx);
            for (int i = 0; i < p1.Count; i++) ret.Add(p1[i]);
            for (int i = 0; i < p2.Count; i++) ret.Add(p2[i]);
            return ret;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Point minPoint = points[0], maxPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X > maxPoint.X || (points[i].X == maxPoint.X && points[i].Y > maxPoint.Y)) maxPoint = points[i];
                if (points[i].X < minPoint.X || (points[i].X == minPoint.X && points[i].Y < minPoint.Y)) minPoint = points[i];
            }
            outPoints.Add(minPoint);
            List<Point> lft1 = left_points(minPoint, maxPoint, points);
            List<Point> lft2 = left_points(maxPoint, minPoint, points);
            List<Point> p1 = quickHullRecursive(minPoint, maxPoint, lft1);
            List<Point> p2 = quickHullRecursive(maxPoint, minPoint, lft2);
            outPoints = list_concat(maxPoint, p1, p2);
        }
        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
