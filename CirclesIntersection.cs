using System;
using System.Drawing;
					
public class Program
{
	
	private static int getIntersections(PointF center0,float radius0,PointF center1, float radius1,out PointF intersection1, out PointF intersection2)
	{
		float cx0 = center0.X,cx1 = center1.X;
		float cy0 = center0.Y,cy1 = center1.Y;
		
		// Find the distance between the centers.
		float dx = cx0 - cx1;
		float dy = cy0 - cy1;
		double dist = Math.Sqrt(dx * dx + dy * dy);

		// See how many solutions there are.
		if (dist > radius0 + radius1)
		{
			// No solutions, the circles are too far apart.
			intersection1 = new PointF(float.NaN, float.NaN);
			intersection2 = new PointF(float.NaN, float.NaN);
			return 0;
		}
		else if (dist < Math.Abs(radius0 - radius1))
		{
			// No solutions, one circle contains the other.
			intersection1 = new PointF(float.NaN, float.NaN);
			intersection2 = new PointF(float.NaN, float.NaN);
			return 0;
		}
		else if ((dist == 0) && (radius0 == radius1))
		{
			// No solutions, the circles coincide.
			intersection1 = new PointF(float.NaN, float.NaN);
			intersection2 = new PointF(float.NaN, float.NaN);
			return 0;
		}
		else
		{
			// Find a and h.
			double a = (radius0 * radius0 -
				radius1 * radius1 + dist * dist) / (2 * dist);
			double h = Math.Sqrt(radius0 * radius0 - a * a);

			// Find P2.
			double cx2 = cx0 + a * (cx1 - cx0) / dist;
			double cy2 = cy0 + a * (cy1 - cy0) / dist;

			// Get the points P3.
			intersection1 = new PointF(
				(float)(cx2 + h * (cy1 - cy0) / dist),
				(float)(cy2 - h * (cx1 - cx0) / dist));
			intersection2 = new PointF(
				(float)(cx2 - h * (cy1 - cy0) / dist),
				(float)(cy2 + h * (cx1 - cx0) / dist));

			// See if we have 1 or 2 solutions.
			if (dist == radius0 + radius1) return 1;
			return 2;
		}
	}
	
	public static void Main()
	{
		PointF center0 = new PointF(15,5),center1 = new PointF(15,10);
		float radius0 = 5,radius1 = 5;
		PointF intersection1,intersection2;
		//The Number of intersecion points
		int n = getIntersections(center0,radius0,center1,radius1,out intersection1,out intersection2);
		switch(n)
		{
			case 1:
				Console.WriteLine("The 2 circles have one intersection point");
				Console.WriteLine("intersection point: "+intersection1.X+" "+intersection1.Y);
				break;
			case 2:
				Console.WriteLine("The 2 circles have two intersection points");
				Console.WriteLine("intersection point 1: "+intersection1.X+" "+intersection1.Y);
				Console.WriteLine("intersection point 2: "+intersection2.X+" "+intersection2.Y);
				break;
			default:
				Console.WriteLine("The 2 circles have no intersection points");
				break;
		}
		
	}
}
