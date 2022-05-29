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
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[0].Y || (points[i].Y == points[0].Y && points[i].X < points[0].X))
                {
                    Point tempP = points[0];
                    points[0] = points[i];
                    points[i] = tempP;
                }
            } 
            for(int i = 1; i < points.Count; i++)
            {
                Point tmp = points[0].Vector(points[i]);
                double ang = Math.Atan2(tmp.Y, tmp.X);
                if (ang < 0) ang += Math.PI;
                ang = (ang * 180) / Math.PI;
                points[i].Ang = ang;
                points[i].Flag = false;
            }
            points[0].Flag = false;
            points.Sort();
            List<Point> temp = new List<Point>();
            temp.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                while (i < points.Count - 1)
                {
                    if (HelperMethods.CheckTurn(points[0].Vector(points[i]), points[0].Vector(points[i + 1])) != Enums.TurnType.Colinear)
                        break;
                    i++;
                }
                temp.Add(points[i]);
            }
            if(temp.Count <= 3)
            {
                outPoints = temp;
                return;
            }
            Stack<Point> graham_st = new Stack<Point>();
            for(int i = 0; i <= 2; i++)
                graham_st.Push(temp[i]);
            for (int i = 3; i < temp.Count; i++)
            {
                while (true)
                {
                    Point a_i = graham_st.Peek();
                    graham_st.Pop();
                    Point a_i_minus_1 = graham_st.Peek();
                    graham_st.Push(a_i);
                    if (HelperMethods.CheckTurn(a_i_minus_1.Vector(a_i), a_i_minus_1.Vector(temp[i])) == Enums.TurnType.Left)
                        break;
                    graham_st.Pop();
                }
                graham_st.Push(temp[i]);
                if(i == temp.Count - 1)
                {
                    while (graham_st.Count != 0)
                    {
                        outPoints.Add(graham_st.Peek());
                        graham_st.Pop();
                    }
                    return;
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
