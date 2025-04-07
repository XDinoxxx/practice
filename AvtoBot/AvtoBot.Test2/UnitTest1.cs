using FluentAssertions;

namespace AvtoBot.Test2;
public class Tests
{
    private ExpressionEvaluator evaluator;

    [SetUp]
    public void Setup()
    {
        evaluator = new ExpressionEvaluator();
    }

    [Test]
    public void TestBasicOperations()
    {
        var additionResult = evaluator.EvaluateExpression("2+3");
        additionResult.Should().Be("5");

        var subsctractResult = evaluator.EvaluateExpression("2-3");
        subsctractResult.Should().Be("-1");

        var multiplaceResult = evaluator.EvaluateExpression("-4*5");
        multiplaceResult.Should().Be("-20");

        var divisionResult = evaluator.EvaluateExpression("-6 / 2");
        divisionResult.Should().Be("-3");
    }

    [Test]
    public void TestVariables()
    {
        var addXResult = evaluator.AddVariable("x", 10);
        addXResult.Should().BeTrue();

        var addYResult = evaluator.AddVariable("y", 2.5);
        addYResult.Should().BeTrue();
    }

    [Test]
    public void TestFunctions()
    {
        var addfFunction = evaluator.AddFunction("f", new string[] { "x", "y", "z" }, "x+y+z");
        addfFunction.Should().BeTrue();
    }

    [Test]
    public void TestExBasicOperation()
    {
        var addResult = evaluator.EvaluateExpression("5+9");
        addResult.Should().NotBe("13");

        var addSymResult = evaluator.EvaluateExpression("5+i");
        addSymResult.Should().Be("Ошибка: Введите корректное значение выражения");

        var symResult = evaluator.EvaluateExpression("vgbhjn");
        symResult.Should().Be("Ошибка: Введите корректное значение выражения");
    }

    [Test]
    public void TestExAddVariables()
    {
        var semicolonResult = evaluator.AddVariable(";", 10);
        semicolonResult.Should().BeFalse();

        var numberResult = evaluator.AddVariable("6", 9);
        numberResult.Should().BeFalse();

        var equalResult = evaluator.AddVariable("=", 111111.1111);
        equalResult.Should().BeFalse();
    }

    [Test]
    public void TestExAddFunctions()
    {
        var gSixFunctionResult = evaluator.AddFunction("g6", new string[] { "x" }, "x+10");
        gSixFunctionResult.Should().BeFalse();

        var multiSemicolonFunctionResult = evaluator.AddFunction(";;;;;", new string[] { ";", "'" }, ";*'/0");
        multiSemicolonFunctionResult.Should().BeFalse();

        var numberParamResult = evaluator.AddFunction("y", new string[] { "6", "0", "yu" }, "6 + 0 + yu / 0");
        numberParamResult.Should().BeTrue();
    }

    [Test]
    public void TestCalculate()
    {
        evaluator.variables["x"] = 5;
        evaluator.functions["f"] = (new[] { "x" }, "2*x + 2");

        var result = evaluator.EvaluateExpression("f(x)");
        result.Should().Be("12");

        evaluator.functions["inf"] = (new[] { "x", "y" }, "x + y / 0");

        var resultInf = evaluator.EvaluateExpression("inf(2,6)");
        resultInf.Should().Be("∞");
    }
}
