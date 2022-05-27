using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class HelperMethods
    {
        public static Enums.PointInPolygon PointInTriangle(Point p, Point a, Point b, Point c)
        {
            if (a.Equals(b) && b.Equals(c))
            {
                if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                    return Enums.PointInPolygon.OnEdge;
                else
                    return Enums.PointInPolygon.Outside;
            }

            Line ab = new Line(a, b);
            Line bc = new Line(b, c);
            Line ca = new Line(c, a);

            if (GetVector(ab).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(bc).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(ca).Equals(Point.Identity)) return (PointOnSegment(p, ab.Start, ab.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == Enums.TurnType.Colinear)
                return PointOnSegment(p, a, b)? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(bc, p) == Enums.TurnType.Colinear && PointOnSegment(p, b, c))
                return PointOnSegment(p, b, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(ca, p) == Enums.TurnType.Colinear && PointOnSegment(p, c, a))
                return PointOnSegment(p, a, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == CheckTurn(bc, p) && CheckTurn(bc, p) == CheckTurn(ca, p))
                return Enums.PointInPolygon.Inside;
            return Enums.PointInPolygon.Outside;
        }
        public static Enums.TurnType CheckTurn(Point vector1, Point vector2)
        {
            double result = CrossProduct(vector1, vector2);
            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }
        public static double CrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static bool PointOnRay(Point p, Point a, Point b)
        {
            if (a.Equals(b)) return true;
            if (a.Equals(p)) return true;
            var q = a.Vector(p).Normalize();
            var w = a.Vector(b).Normalize();
            return q.Equals(w);
        }
        public static bool PointOnSegment(Point p, Point a, Point b)
        {
            if (a.Equals(b))
                return p.Equals(a);

            if (b.X == a.X)
                return p.X == a.X && (p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y));
            if (b.Y == a.Y)
                return p.Y == a.Y && (p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X));
            double tx = (p.X - a.X) / (b.X - a.X);
            double ty = (p.Y - a.Y) / (b.Y - a.Y);

            return (Math.Abs(tx - ty) <= Constants.Epsilon && tx <= 1 && tx >= 0);
        }
        /// <summary>
        /// Get turn type from cross product between two vectors (l.start -> l.end) and (l.end -> p)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Enums.TurnType CheckTurn(Line l, Point p)
        {
            Point a = l.Start.Vector(l.End);
            Point b = l.End.Vector(p);
            return HelperMethods.CheckTurn(a, b);
        }
        public static Point GetVector(Line l)
        {
            return l.Start.Vector(l.End);
        }

        //me
        public static double Dist(Point p1, Point p2) //can implement it without sqrt
        {
            double dx = Math.Abs(p1.X - p2.X);
            double dy = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static int CCW(Point a, Point b, Point c)
        {
            Point v1 = a.Vector(b), v2 = a.Vector(c);
            double t = CrossProduct(v1, v2);
            if (t > +1e-6) return 1;
            if (t < -1e-6) return -1;
            if (v1.X * v2.X < -1e-6 || v1.Y * v2.Y < -1e-6)
                return -1;
            Point O = Point.Identity;
            if (Dist(v1, O) < Dist(v2, O) - 1e-6)
                return 1;
            return 0;
        }
        public static bool Is_segment_intersection(Line a, Line b)
        {
            return CCW(a.Start, a.End, b.Start) * CCW(a.Start, a.End, b.End) <= 0 &&
                    CCW(b.Start, b.End, a.Start) * CCW(b.Start, b.End, a.End) <= 0;
        }
        public static Point Get_intersection(Line e, Line f)
        {
            double a1 = e.End.X - e.Start.X, a2 = e.End.Y - e.Start.Y;
            double b1 = f.Start.X - f.End.X, b2 = f.Start.Y - f.End.Y;
            double c1 = e.Start.X - f.Start.X, c2 = e.Start.Y - f.Start.Y;
            Point ab1 = new Point(a1, b1);
            Point ab2 = new Point(a2, b2);
            Point bc1 = new Point(b1, c1);
            Point bc2 = new Point(b2, c2);
            double t1 = CrossProduct(bc1, bc2) / CrossProduct(ab1, ab2);
            return new Point(e.Start.X + (e.End.X - e.Start.X) * t1, e.Start.Y + (e.End.Y - e.Start.Y) * t1);
        }
    }
}
