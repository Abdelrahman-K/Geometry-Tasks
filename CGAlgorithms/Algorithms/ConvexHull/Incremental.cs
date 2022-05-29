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
        public int getNumb(int x, bool next)
        {
            if (next) return (x + 1) % 3;
            else return (x - 1 + 3) % 3;
        }
        public List<Point> buildSolution(int tempIdx, List<Point> points, int[] nextPoint)
        {
            List<Point> ret = new List<Point>();
            do
            {
                ret.Add(points[tempIdx]);
                tempIdx = nextPoint[tempIdx];
            } while (points[tempIdx] != ret[0]);
            return ret;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 0; i < points.Count; i++) points[i].Flag = true;
            points.Sort();
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i - 1].Equals(points[i]) ||
                    (i + 1 < points.Count && points[i].X - points[i - 1].X < 1e-6 && points[i + 1].X - points[i].X < 1e-6))
                {
                    points.RemoveAt(i);
                    i--;
                }
            }
            if(points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            int[] prevPoint = new int[points.Count];
            int[] nextPoint = new int[points.Count];
            Line line = new Line(points[0], points[1]);
            for (int idx = 0; idx <= 2; idx++)
            {
                nextPoint[idx] = getNumb(idx, (HelperMethods.CheckTurn(line, points[2]) != Enums.TurnType.Right) ? true : false);
                prevPoint[idx] = getNumb(idx, (HelperMethods.CheckTurn(line, points[2]) != Enums.TurnType.Right) ? false : true);
            }
            int tempIdx = 2, upper, lower;
            for (int i = 3; i < points.Count; i++)
            {
                while (true)
                {
                    line = new Line(points[i], points[tempIdx]);
                    if (HelperMethods.CheckTurn(line, points[nextPoint[tempIdx]]) == Enums.TurnType.Left)
                    {
                        upper = tempIdx;
                        break;
                    }
                    tempIdx = nextPoint[tempIdx];
                }
                tempIdx = i - 1;
                while (true)
                {
                    line = new Line(points[i], points[tempIdx]);
                    if (HelperMethods.CheckTurn(line, points[prevPoint[tempIdx]]) == Enums.TurnType.Right)
                    {
                        lower = tempIdx;
                        break;
                    }
                    tempIdx = prevPoint[tempIdx];
                }
                prevPoint[upper] = i;
                nextPoint[lower] = i;
                nextPoint[i] = upper;
                prevPoint[i] = lower;
                tempIdx = i;
            }
            outPoints = buildSolution(tempIdx, points, nextPoint);
            
        }
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
