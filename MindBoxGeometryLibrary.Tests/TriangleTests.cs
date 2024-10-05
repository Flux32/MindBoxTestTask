using GeometryLibraryTest.Shapes;

namespace GeometryLibraryTest.Tests;

public class TriangleTests
{
    [Fact]
    public void CalculateArea_TriangleWithEdges_3_4_5_ShouldReturn6()
    {
        Triangle triangle = new Triangle(3, 4, 5);
        
        double area = triangle.CalculateArea();
        
        Assert.Equal(6, area, precision: 10);
    }

    [Fact]
    public void IsRightAngled_RightAngledTriangle_ReturnTrue()
    {
        Triangle triangle = new Triangle(3, 4, 5);
        
        bool isRightAngled = triangle.IsRightAngled();
        
        Assert.True(isRightAngled);
    }
    
    [Fact]
    public void IsRightAngled_NotRightAngledTriangle_ReturnFalse()
    {
        Triangle triangle = new Triangle(3, 4, 6);
        
        bool isRightAngled = triangle.IsRightAngled();
        
        Assert.False(isRightAngled);
    }
}