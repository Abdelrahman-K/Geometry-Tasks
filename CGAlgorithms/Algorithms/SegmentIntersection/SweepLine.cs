using CGUtilities;
using CGUtilities.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine:Algorithm
    {
        class Event : IComparable
        {
            public int Type { get; set; }
            public int Ind { get; set; }
            public int Ind2 { get; set; }
            public double X_value { get; set; }
            
            public Event(int Type, int Ind, int Ind2, double X_value)
            {
                this.Type = Type;
                this.Ind = Ind;
                this.Ind2 = Ind2;
                this.X_value = X_value;
            }
            public int CompareTo(object obj)
            {
                return X_value.CompareTo(((Event) obj).X_value);
            }
        };
        class Element : IComparable
        {
            public int Ind { get; set; }
            public double Y_value { get; set; }

            public Element(int Ind, double Y_value)
            {
                this.Ind = Ind;
                this.Y_value = Y_value;
            }
            public int CompareTo(object obj)
            {
                return Y_value.CompareTo(((Element) obj).Y_value);
            }
        };
        
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            OrderedSet<Event> events = new OrderedSet<Event>();
            OrderedSet<Element> window = new OrderedSet<Element>();
            List<double> Y_values = new List<double>();
            for (int i = 0; i < lines.Count; i++)
            {
                events.Add(new Event(1, i, i, lines[i].Start.X));
                events.Add(new Event(2, i, i, lines[i].End.X));
                Y_values.Add(lines[i].Start.Y);
            }
            while (events.Count != 0)
            {
                Event e = events.GetFirst();
                events.RemoveFirst();
                if (e.Type == 1) //entry
                {
                    Element tmp = new Element(e.Ind, Y_values[e.Ind]);
                    var bounds = window.DirectUpperAndLower(tmp);
                    window.Add(tmp);
                    Element ub = bounds.Key, lb = bounds.Value;
                    if (ub != default(Element) && lb != default(Element) && HelperMethods.Is_segment_intersection(lines[ub.Ind], lines[lb.Ind]))
                    {
                        Point intersection = HelperMethods.Get_intersection(lines[ub.Ind], lines[lb.Ind]);
                        events.Remove(new Event(3, ub.Ind, lb.Ind, intersection.X));
                    }
                    if (ub != default(Element) && HelperMethods.Is_segment_intersection(lines[ub.Ind], lines[e.Ind]))
                    {
                        Point intersection = HelperMethods.Get_intersection(lines[ub.Ind], lines[e.Ind]);
                        events.Add(new Event(3, ub.Ind, e.Ind, intersection.X));
                    }
                    if (lb != default(Element) && HelperMethods.Is_segment_intersection(lines[e.Ind], lines[lb.Ind]))
                    {
                        Point intersection = HelperMethods.Get_intersection(lines[e.Ind], lines[lb.Ind]);
                        events.Add(new Event(3, e.Ind, lb.Ind, intersection.X));
                    }
                    
                }
                else if (e.Type == 2) //exit
                {
                    Element tmp = new Element(e.Ind, Y_values[e.Ind]);
                    window.Remove(tmp);
                    var bounds = window.DirectUpperAndLower(tmp);
                    Element ub = bounds.Key, lb = bounds.Value;
                    if (ub != default(Element) && lb != default(Element) && HelperMethods.Is_segment_intersection(lines[ub.Ind], lines[lb.Ind]))
                    {
                        Point intersection = HelperMethods.Get_intersection(lines[ub.Ind], lines[lb.Ind]);
                        events.Add(new Event(3, ub.Ind, lb.Ind, intersection.X));
                    }
                }
                else //intersection
                {
                    Point intersection = HelperMethods.Get_intersection(lines[e.Ind], lines[e.Ind2]);
                    outPoints.Add(intersection);
                    //
                    Element tmp = new Element(e.Ind, Y_values[e.Ind]);
                    Element tmp2 = new Element(e.Ind2, Y_values[e.Ind2]);
                    Element ub = window.DirectUpperAndLower(tmp).Key, lb = window.DirectUpperAndLower(tmp2).Value;
                    window.Remove(tmp);
                    window.Remove(tmp2);
                    if (ub != default(Element) && HelperMethods.Is_segment_intersection(lines[ub.Ind], lines[tmp.Ind]))
                    {
                        intersection = HelperMethods.Get_intersection(lines[ub.Ind], lines[tmp.Ind]);
                        events.Remove(new Event(3, ub.Ind, tmp.Ind, intersection.X));
                    }
                    if (lb != default(Element) && HelperMethods.Is_segment_intersection(lines[tmp2.Ind], lines[lb.Ind]))
                    {
                        intersection = HelperMethods.Get_intersection(lines[tmp2.Ind], lines[lb.Ind]);
                        events.Remove(new Event(3, tmp2.Ind, lb.Ind, intersection.X));
                    }
                    if (ub != default(Element) && HelperMethods.Is_segment_intersection(lines[ub.Ind], lines[tmp2.Ind]))
                    {
                        intersection = HelperMethods.Get_intersection(lines[ub.Ind], lines[tmp2.Ind]);
                        events.Add(new Event(3, ub.Ind, tmp2.Ind, intersection.X));
                    }
                    if (lb != default(Element) && HelperMethods.Is_segment_intersection(lines[tmp.Ind], lines[lb.Ind]))
                    {
                        intersection = HelperMethods.Get_intersection(lines[tmp.Ind], lines[lb.Ind]);
                        events.Add(new Event(3, tmp.Ind, lb.Ind, intersection.X));
                    }
                    //
                    double swp = Y_values[e.Ind];
                    Y_values[e.Ind] = Y_values[e.Ind2];
                    Y_values[e.Ind2] = swp;
                    tmp = new Element(e.Ind, Y_values[e.Ind]);
                    tmp2 = new Element(e.Ind2, Y_values[e.Ind2]);
                    window.Add(tmp);
                    window.Add(tmp2);
                }
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
