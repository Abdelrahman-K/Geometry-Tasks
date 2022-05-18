using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[0].Y || (points[i].Y == points[0].Y && points[i].X < points[0].X))
                {
                    Point tmp = points[0];
                    points[0] = points[i];
                    points[i] = tmp;
                }
            }
            //remove duplicates
            List<bool> vis = new List<bool>();
            for (int i = 0; i < points.Count; i++)
            {
                bool f = false;
                for (int j = 0; j < i; j++)
                {
                    if (points[i].Equals(points[j]))
                        f = true;
                }
                vis.Add(f);
            }
            Point prev = points[0];
            do
            {
                outPoints.Add(prev);
                Point cur = prev;
                for (int i = 0; i < points.Count; i++)
                {
                    if (vis[i]) continue;
                    if (prev == cur)
                        cur = points[i];
                    Line l = new Line(prev, cur);
                    var f = HelperMethods.CheckTurn(l, points[i]);
                    if (f == Enums.TurnType.Right ||
                        (f == Enums.TurnType.Colinear && 
                            Math.Abs(HelperMethods.Dist(prev, cur) + HelperMethods.Dist(cur, points[i]) - HelperMethods.Dist(prev, points[i])) < 1e-6))
                        cur = points[i];
                }
                prev = cur;
            } while (prev != points[0]);
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
