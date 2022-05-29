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
            int idxForLeftmost = 0;
            for (int i = 1; i < points.Count; i++)
                if (points[i].X < points[idxForLeftmost].X) idxForLeftmost = i;

            int idx1 = idxForLeftmost, idx2;
            do
            {
                outPoints.Add(points[idx1]);
                idx2 = (idx1 + 1) % points.Count;
                for (int i = 0; i < points.Count; i++)
                {
                    Line line = new Line(outPoints.Last(), points[idx2]);
                    var f = HelperMethods.CheckTurn(line, points[i]);
                    if (f == Enums.TurnType.Right ||
                       (f == Enums.TurnType.Colinear && Math.Abs(HelperMethods.Distance(outPoints.Last(), points[idx2]) + HelperMethods.Distance(points[idx2], points[i]) - HelperMethods.Distance(outPoints.Last(), points[i])) < 1e-5))
                        idx2 = i;
                }
                idx1 = idx2;

            } while (idx1 != idxForLeftmost); 
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
