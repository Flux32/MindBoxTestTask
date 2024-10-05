namespace GeometryLibraryTest.Shapes;

public class Triangle : IShape
{
    public Triangle(double ab, double bc, double ac)
    {
        Ab = ab;
        Bc = bc;
        Ac = ac;
    }

    public double Ab { get; } 
    public double Bc { get; }
    public double Ac { get; }
    
    public double CalculateArea()
    {
        double p = (Ab + Bc + Ac) / 2;
        
        return Math.Sqrt(p * (p - Ab) * (p - Bc) * (p - Ac));
    }

    public bool IsRightAngled()
    {
        double[] edges = [Ab, Bc, Ac];

        Array.Sort(edges);
        
        const double tolerance = 1E-10;
        return Math.Abs(Math.Pow(edges[2], 2) - (Math.Pow(edges[0], 2) + Math.Pow(edges[1], 2))) < tolerance;
    }
}