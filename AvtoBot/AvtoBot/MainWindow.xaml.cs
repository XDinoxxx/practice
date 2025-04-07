using System.Data;
using System.Text.RegularExpressions;
using System.Windows;

namespace AvtoBot;

public partial class MainWindow : Window
{
    private ExpressionEvaluator evaluator = new ExpressionEvaluator();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Calculate(object sender, RoutedEventArgs e)
    {
        ResultText.Text = "Результат: \n" + evaluator.EvaluateExpression(InputExpression.Text);
    }

    private void Add_Variable(object sender, RoutedEventArgs e)
    {
        try
        {
            string input = InputExpression.Text;
            var match = Regex.Match(input, @"^\s*([a-zA-Z]+)\s*=\s*([\d\.]+)\s*$");

            if (match.Success)
            {
                string varName = match.Groups[1].Value;
                double varValue = double.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

                if (evaluator.AddVariable(varName, varValue))
                {
                    ResultText.Text = $"Переменная {varName} = {varValue} добавлена";
                }

                Variables.Text = string.Join("\n",evaluator.variables.Select(v => $"{v.Key} = {v.Value}"));
            }
            else
            {
                ResultText.Text = "Ошибка! Введите переменную в формате x = 5";
            }
        }
        catch (Exception)
        {
            ResultText.Text = "Ошибка при добавлении переменной";
        }
    }

    private void Add_Function(object sender, RoutedEventArgs e)
    {
        try
        {
            string input = InputExpression.Text;
            var match = Regex.Match(input, @"^\s*([a-zA-Z]+)\(([^)]*)\)\s*=\s*(.+)$");

            if (match.Success)
            {
                string funcName = match.Groups[1].Value;
                string[] parameters = match.Groups[2].Value.Split(',').Select(p => p.Trim()).ToArray();
                string expression = match.Groups[3].Value;

                expression = FixFunction(expression);

                if (evaluator.AddFunction(funcName, parameters, expression))
                {
                    ResultText.Text = $"Функция {funcName}({string.Join(", ", parameters)}) добавлена";
                }
                
                Functions.Text = string.Join("\n",evaluator.functions.Select(f => $"{f.Key}({string.Join(", ", f.Value.parameters)}) = {BeautifyExpression(f.Value.expression)}"));

            }
            else
            {
                ResultText.Text = "Ошибка! Введите функцию в формате f(x) = 5x - x";
            }
        }
        catch (Exception)
        {
            ResultText.Text = "Ошибка при добавлении функции";
        }
    }

    
    private string FixFunction(string expression)
    {
        return Regex.Replace(expression, @"(\d)([a-zA-Z])", "($1*$2)");
    }
    private string BeautifyExpression(string expression)
    {
        return Regex.Replace(expression, @"(\d)\s*\*\s*([a-zA-Z])", "$1$2");
    }
}