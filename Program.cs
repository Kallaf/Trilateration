using System;
using System.Drawing;
					
public class Program
{
	
	private static int intersectionOf2Circles(PointF center0,float radius0,PointF center1, float radius1,out PointF intersection1, out PointF intersection2)
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
	
	
	private static float GetDistance(PointF point1, PointF point2)
	{
		float a = point2.X - point1.X;
		float b = point2.Y - point1.Y;

		return (float)Math.Sqrt(a * a + b * b);
	}
	
	private static PointF intersectionOf3Circles(PointF[] IC0C1,PointF[] IC0C2,PointF[] IC1C2,float radius0,float radius1,float radius2)
	{
		// Select between intersection point 1 and intersection point2 for each two circles using bit manpulation technique.
		int best_premiutation = 0;
		float min = float.MaxValue;
		for(int i=0;i<8;i++)
		{
			float d1 = GetDistance(IC0C1[i&1],IC0C2[(i>>1)&1]);
			float d2 = GetDistance(IC0C1[i&1],IC1C2[(i>>2)&1]);
			float d3 = GetDistance(IC0C2[(i>>1)&1],IC1C2[(i>>2)&1]);
			float sum = d1+d2+d3;
			if(sum < min)
			{
				min = sum;
				best_premiutation = i;
			}
		}
		
		//Get the midpoint of the 3 intersections
		float userX = (IC0C1[best_premiutation&1].X+IC0C2[(best_premiutation>>1)&1].X+IC1C2[(best_premiutation>>2)&1].X)/3;
		float userY = (IC0C1[best_premiutation&1].Y+IC0C2[(best_premiutation>>1)&1].Y+IC1C2[(best_premiutation>>2)&1].Y)/3;
		return new PointF(userX,userY);
	}
	
	public static bool getUserLocation(PointF center0,float radius0,PointF center1, float radius1,PointF center2, float radius2,out PointF userLocation)
	{
		// Each array represent the 2 intersecting points between 2 circles.
		
		// Intersecting point between circle0  and circle1.
		PointF[] IC0C1 = new PointF[2];
		
		// Intersecting point between circle0  and circle2.
		PointF[] IC0C2 = new PointF[2];
		
		// Intersecting point between circle1  and circle2.
		PointF[] IC1C2 = new PointF[2];
		
		// Where ni is the number of intersecting points
		int n1 = intersectionOf2Circles(center0,radius0,center1,radius1,out IC0C1[0],out IC0C1[1]);
		int n2 = intersectionOf2Circles(center0,radius0,center2,radius2,out IC0C2[0],out IC0C2[1]);
		int n3 = intersectionOf2Circles(center1,radius1,center2,radius2,out IC1C2[0],out IC1C2[1]);
		
		// If ni == 0 this means there is no intersecting points between 2 circles as there is an error happened while calculating the distance.
		// Fixing error should be handled later.
		if(n1 == 0 || n2 == 0 || n3 == 0)
		{
			userLocation = new PointF(float.NaN, float.NaN);
			return false;
		}
		userLocation = intersectionOf3Circles(IC0C1,IC0C2,IC1C2,radius0,radius1,radius2);
		return true;
	}
	
	public static void Main()
	{
		// Define the Three wifi locations
		PointF accessPoint0 = new PointF(45,5),accessPoint1 = new PointF(15,10.54f),accessPoint2 = new PointF(7,7.5f);
		// The Actual location of the user
		PointF actualUserLocation = new PointF(12,3.34f);
		
		// Calculating the distance between the user and the three access points
		float distance0 = GetDistance(actualUserLocation,accessPoint0);
		float distance1 = GetDistance(actualUserLocation,accessPoint1);
		float distance2 = GetDistance(actualUserLocation,accessPoint2);
		
		Console.WriteLine("Current user location: "+actualUserLocation.X+","+actualUserLocation.Y);
		
		// Predict the user location using the position of the access points and the distances between them and the user.
		PointF predictedUserLocation;
		bool canSpecifyLocation = getUserLocation(accessPoint0,distance0,accessPoint1,distance1,accessPoint2,distance2,out predictedUserLocation);
		
		if(canSpecifyLocation)
			Console.WriteLine("Predicted user location: "+predictedUserLocation.X+","+predictedUserLocation.Y);
		else
			Console.WriteLine("Cannot determine the user Location");
	}
}
