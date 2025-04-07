using NUnit.Framework;
using NUnit;
using NUnit.Framework.Legacy;

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
        Assert.That(evaluator.EvaluateExpression("2+3"), Is.EqualTo("5"));
        Assert.That(evaluator.EvaluateExpression("2-2"), Is.EqualTo("0"));
        Assert.That(evaluator.EvaluateExpression("5/0"), Is.EqualTo("∞"));
        Assert.That(evaluator.EvaluateExpression("2*32"), Is.EqualTo("64"));
    }

    [Test]
    public void TestVariables()
    {
        Assert.That(evaluator.AddVariable("x", 10), Is.True);
        Assert.That(evaluator.EvaluateExpression("x+10"), Is.EqualTo("20"));
        Assert.That(evaluator.AddVariable("y", 5), Is.True);
        Assert.That(evaluator.EvaluateExpression("x+y"), Is.EqualTo("15"));
        Assert.That(evaluator.AddVariable("ч", 15), Is.False);
    }

    [Test]
    public void TestFunctions()
    {
        Assert.That(evaluator.AddFunction("f",new string[] { "x", "y", "z" }, "x+y+z"), Is.True);
        Assert.That(evaluator.EvaluateExpression("f(1,2,3)"), Is.EqualTo("6"));
        Assert.That(evaluator.AddVariable("x", 10), Is.True);
        Assert.That(evaluator.AddVariable("y", 20), Is.True);
        Assert.That(evaluator.AddVariable("z", 30), Is.True);
        Assert.That(evaluator.EvaluateExpression("f(x,y,z)"), Is.EqualTo("60"));
    }

    [Test]
    public void TestExBasicOperation()
    {
        Assert.That(evaluator.EvaluateExpression("5+9"), Is.Not.EqualTo("13"));
        Assert.That(evaluator.EvaluateExpression("5+i"), Is.EqualTo("Ошибка: Введите корректное значение выражения"));
        Assert.That(evaluator.EvaluateExpression("vgbhjn"), Is.EqualTo("Ошибка: Введите корректное значение выражения"));
    }

    [Test]
    public void TestExAddVariables()
    {
        Assert.That(evaluator.AddVariable(";", 10), Is.False);
        Assert.That(evaluator.AddVariable("6", 9), Is.False);
        Assert.That(evaluator.AddVariable("=", 1111111111), Is.False);
    }

    [Test]
    public void TestExAddFunctions()
    {
        Assert.That(evaluator.AddFunction("g6", new string[] { "x" }, "x+10"), Is.False);
        Assert.That(evaluator.AddFunction(";;;;;", new string[] { ";", "'" }, ";*'/0"), Is.False);
        Assert.That(evaluator.AddFunction("x6", new string[] { "x", "7" }, "x+7+7"), Is.False);
        Assert.That(evaluator.AddFunction("y", new string[] { "6", "0", "yu" }, "6 + 0 + yu / 0"), Is.True);
        Assert.That(evaluator.EvaluateExpression("y(6,0,10)"), Is.EqualTo("∞"));
    }
}
