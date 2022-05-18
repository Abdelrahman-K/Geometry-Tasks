using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
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
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j || vis[i] || vis[j]) continue;
                    int cnt1 = 0, cnt2 = 0;
                    for (int p = 0; p < points.Count; p++)
                    {
                        Line l = new Line(points[i], points[j]);
                        var ch = HelperMethods.CheckTurn(l, points[p]);
                        if (ch == Enums.TurnType.Right)
                            cnt1++;
                        else if (ch == Enums.TurnType.Left)
                            cnt2++;
                        else
                        {
                            if (Math.Abs(HelperMethods.Dist(points[i], points[p]) + HelperMethods.Dist(points[p], points[j]) - HelperMethods.Dist(points[i], points[j])) > 1e-6)
                            {
                                cnt1++;
                                cnt2++;
                            }
                        }
                    }
                    if (cnt1 == 0 || cnt2 == 0)
                    {
                        outPoints.Add(points[i]);
                        break;
                    }
                }
            }
            if (points.Count <= 3)
                outPoints = points;

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
