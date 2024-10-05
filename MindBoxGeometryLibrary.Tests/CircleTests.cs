using GeometryLibraryTest.Figures;

namespace GeometryLibraryTest.Tests;

public class CircleTests
{
    [Theory]
    [InlineData(5, Math.PI * 25)]
    [InlineData(10, Math.PI * 100)]
    [InlineData(12, Math.PI * 144)]
    public void CalculateArea_CircleWithValidRadius_ShouldReturnCorrectValue(double radius, double expected)
    {
        Circle circle = new Circle(radius);
        
        double area = circle.CalculateArea();

        Assert.Equal(expected, area, precision: 10);
    }
}