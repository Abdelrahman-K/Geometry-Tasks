using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
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
            for (int p = 0; p < points.Count; p++)
            {
                bool isValid = true;
                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        for (int k = j + 1; k < points.Count; k++)
                        {
                            if (i == p || j == p || k == p) continue;
                            if (HelperMethods.PointInTriangle(points[p], points[i], points[j], points[k]) != Enums.PointInPolygon.Outside)
                                isValid = false;
                        }
                    }
                }
                if (isValid) outPoints.Add(points[p]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
