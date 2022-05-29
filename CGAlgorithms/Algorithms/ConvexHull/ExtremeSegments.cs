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
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (points[i].Equals(points[j]))
                    {
                        points.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j) continue;
                    bool right = false, left = false, colinear = false;
                    for (int p = 0; p < points.Count; p++)
                    {
                        if (p == i || p == j) continue;
                        Line line = new Line(points[i], points[j]);
                        if (HelperMethods.CheckTurn(line, points[p]) == Enums.TurnType.Left) left = true;
                        else if (HelperMethods.CheckTurn(line, points[p]) == Enums.TurnType.Right) right = true;
                        else if (HelperMethods.CheckTurn(line, points[p]) == Enums.TurnType.Colinear)
                            if (Math.Abs(HelperMethods.Distance(points[i], points[p]) + HelperMethods.Distance(points[p], points[j]) - HelperMethods.Distance(points[i], points[j])) > 1e-6)
                                colinear = true;
                    }
                    if (colinear == false && (right == false || left == false))
                    {
                        outPoints.Add(points[i]);
                        break;
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
