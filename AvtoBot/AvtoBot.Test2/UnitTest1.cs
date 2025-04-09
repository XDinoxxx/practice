using FluentAssertions;

namespace AvtoBot.Test2;
public class Tests
{
    private ExpressionEvaluator evaluator;

    [SetUp]
    public void Setup()
    {
        // Arrange
        evaluator = new ExpressionEvaluator();
    }

    [Test]
    public void TestBasicOperations()
    {
        // Act
        var additionResult = evaluator.EvaluateExpression("2+3");
        var subsctractResult = evaluator.EvaluateExpression("2-3");
        var multiplaceResult = evaluator.EvaluateExpression("-4*5");
        var divisionResult = evaluator.EvaluateExpression("-6 / 2");

        // Assert
        additionResult.Should().Be("5");
        subsctractResult.Should().Be("-1");
        multiplaceResult.Should().Be("-20");
        divisionResult.Should().Be("-3");
    }

    [Test]
    public void TestVariables()
    {
        // Act
        var addXResult = evaluator.AddVariable("x", 10);
        var addYResult = evaluator.AddVariable("y", 2.5);

        // Assert
        addXResult.Should().BeTrue();
        addYResult.Should().BeTrue();
    }

    [Test]
    public void TestFunctions()
    {
        // Act
        var addfFunction = evaluator.AddFunction("f", new string[] { "x", "y", "z" }, "x+y+z");

        // Assert
        addfFunction.Should().BeTrue();
    }

    [Test]
    public void TestExBasicOperation()
    {
        // Act
        var addResult = evaluator.EvaluateExpression("5+9");
        var addSymResult = evaluator.EvaluateExpression("5+i");
        var symResult = evaluator.EvaluateExpression("vgbhjn");

        // Assert
        addResult.Should().NotBe("13");
        addSymResult.Should().Be("Ошибка: Введите корректное значение выражения");
        symResult.Should().Be("Ошибка: Введите корректное значение выражения");
    }

    [Test]
    public void TestExAddVariables()
    {
        // Act
        var semicolonResult = evaluator.AddVariable(";", 10);
        var numberResult = evaluator.AddVariable("6", 9);
        var equalResult = evaluator.AddVariable("=", 111111.1111);

        // Assert
        semicolonResult.Should().BeFalse();
        numberResult.Should().BeFalse();
        equalResult.Should().BeFalse();
    }

    [Test]
    public void TestExAddFunctions()
    {
        // Act
        var gSixFunctionResult = evaluator.AddFunction("g6", new string[] { "x" }, "x+10");
        var multiSemicolonFunctionResult = evaluator.AddFunction(";;;;;", new string[] { ";", "'" }, ";*'/0");
        var numberParamResult = evaluator.AddFunction("y", new string[] { "6", "0", "yu" }, "6 + 0 + yu / 0");

        // Assert
        gSixFunctionResult.Should().BeFalse();
        multiSemicolonFunctionResult.Should().BeFalse();
        numberParamResult.Should().BeTrue();
    }

    [Test]
    public void TestCalculate()
    {
        // Arrange
        evaluator.variables["x"] = 5;
        evaluator.functions["f"] = (new[] { "x" }, "2*x + 2");
        evaluator.functions["inf"] = (new[] { "x", "y" }, "x + y / 0");

        // Act
        var result = evaluator.EvaluateExpression("f(x)");
        var resultInf = evaluator.EvaluateExpression("inf(2,6)");

        // Assert
        result.Should().Be("12");
        resultInf.Should().Be("∞");
    }
}
