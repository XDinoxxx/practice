using System.Data;
using System.Text.RegularExpressions;

namespace AvtoBot;

public class ExpressionEvaluator
{
    public Dictionary<string, double> variables = new Dictionary<string, double>();
    public Dictionary<string, (string[] parameters, string expression)> functions = new Dictionary<string, (string[] parameters, string expression)>();

    public string EvaluateExpression(string expression)
    {
        try
        {
            foreach (var variable in variables)
            {
                expression = Regex.Replace(expression, @"\b" + variable.Key + @"\b", variable.Value.ToString());
            }

            expression = ReplaceFunctions(expression);
            expression = expression.Replace(',', '.');

            var result = new DataTable().Compute(expression, null);
            return result.ToString();
        }
        catch (DivideByZeroException)
        {
            return "∞";
        }
        catch (Exception)
        {
            return "Ошибка: Введите корректное значение выражения";
        }
    }

    public bool AddVariable(string name, double value)
    {
        if (Regex.IsMatch(name, "^[a-zA-Z]+$"))
        {
            variables[name] = value;
            return true;
        }
        return false;
    }

    public bool AddFunction(string name, string[] parameters, string expression)
    {
        if (Regex.IsMatch(name, "^[a-zA-Z]+$"))
        {
            functions[name] = (parameters, FixFunction(expression));
            return true;
        }
        return false;
    }

    private string ReplaceFunctions(string expression)
    {
        foreach (var func in functions)
        {
            string funcName = func.Key;
            var (parameters, funcExpression) = func.Value;

            string pattern = $@"\b{funcName}\(([^)]+)\)";

            expression = Regex.Replace(expression, pattern, match =>
            {
                string[] args = match.Groups[1].Value.Split(',').Select(a => a.Trim()).ToArray();

                if (args.Length != parameters.Length)
                    throw new Exception($"Ошибка: функция {funcName} принимает {parameters.Length} аргументов, а передано {args.Length}");

                string replacedExpression = funcExpression;

                for (int i = 0; i < parameters.Length; i++)
                {
                    replacedExpression = Regex.Replace(replacedExpression, $@"\b{parameters[i]}\b", args[i]);
                }

                return $"({replacedExpression})";
            });
        }

        return expression;
    }

    private string FixFunction(string expression)
    {
        return Regex.Replace(expression, @"(\d)([a-zA-Z])", "$1*$2");
    }
}
