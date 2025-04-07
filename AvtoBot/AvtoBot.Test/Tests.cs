using NUnit;
using NUnit.Framework;
using AvtoBot;
using NUnit.Framework.Interfaces;

namespace AvtoBot.Test;

[TestFixture]
public class ExpressionEvaluatorTest
{
    private ExpressionEvaluator evaluator;

    [SetUp]
    public void Setup()
    {
        evaluator = new ExpressionEvaluator();
    }

    [Test]
    public void TestBasicOparation()
    {
        Assert.Equals("5", evaluator.EvaluateExpression("2+3"));
        Assert.Equals("0", evaluator.EvaluateExpression("2-2"));
        Assert.Equals("∞", evaluator.EvaluateExpression("5/0"));
        Assert.Equals("64", evaluator.EvaluateExpression("2*32"));
    }

    [Test]
    public void TestAddVariable()
    {
        Assert.That(evaluator.AddVariable("x", 10));
        Assert.Equals("10", evaluator.EvaluateExpression("x"));
    }
}