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
            //
            for (int p = 0; p < points.Count; p++)
            {
                Console.WriteLine(vis[p]);
                bool valid = true;
                if (vis[p]) continue;
                for (int i = 0; i < points.Count; i++)
                {
                    if (i == p || vis[i]) continue;
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        if (j == p || vis[j]) continue;
                        for (int k = j + 1; k < points.Count; k++)
                        {
                            if (k == p || vis[k]) continue;
                            if (HelperMethods.PointInTriangle(points[p], points[i], points[j], points[k]) != Enums.PointInPolygon.Outside)
                                valid = false;
                        }
                    }
                }
                if (valid && !outPoints.Contains(points[p]))
                    outPoints.Add(points[p]);
            }
            if (points.Count <= 3)
                outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
