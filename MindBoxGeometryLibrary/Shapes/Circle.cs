namespace GeometryLibraryTest.Shapes;

public class Circle : IShape
{
    public Circle(double radius)
    {
        if (radius < 0)
            throw new ArgumentOutOfRangeException($"The {nameof(radius)} must be greater than 0");
        
        Radius = radius;
    }
    
    public double Radius { get; }

    public double CalculateArea() => Math.PI * Math.Pow(Radius, 2);
}