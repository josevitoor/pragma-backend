using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrossCutting.Util
{
    public static class ReversePolishExtension
    {
        private static readonly Dictionary<string, int> precedence = new Dictionary<string, int>
        {
            {"+", 2},
            {"-", 2},
            {"*", 3},
            {"/", 3},
            {"^", 4}
        };

        public static string ToReversePolishString(this string expression)
        {
            return string.Join(' ', expression.ToReversePolish());
        }

        public static Queue<string> ToReversePolish(this string expression)
        {
            var tokens = expression.ParseReversePolishTokens();
            var output = new Queue<string>();
            var operators = new Stack<string>();

            foreach (var token in tokens)
            {
                if (token.IsNumber() || token.IsVariable())
                {
                    output.Enqueue(token);
                }
                else if (token.IsOperator())
                {
                    while (operators.Any() && !operators.Peek().Equals("(")
                        && (precedence[operators.Peek()] > precedence[token]
                            || (precedence[operators.Peek()] == precedence[token] && token.IsLeftAssociative())))
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                }
                else if (token.Equals("("))
                {
                    operators.Push(token);
                }
                else if (token.Equals(")"))
                {

                    while (operators.Any() && !operators.Peek().Equals("("))
                        output.Enqueue(operators.Pop());

                    if (!operators.Any())
                        throw new ArgumentException("Existe um parênteses direito sem um parênteses esquerdo correspondente.");

                    operators.Pop(); // discard left parenthesis
                }
            }

            while (operators.Any())
            {
                if (operators.Peek().Equals("("))
                    throw new ArgumentException("Existe um parênteses esquerdo sem um parênteses direito correspondente.");
                output.Enqueue(operators.Pop());
            }

            return output;
        }

        private static bool IsVariable(this string str) => new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$").IsMatch(str);
        private static bool IsNumber(this string str) => double.TryParse(str, out _);
        private static bool IsOperator(this string str) => precedence.ContainsKey(str);
        private static bool IsLeftAssociative(this string str) => str.IsOperator() && !str.Equals("^");

        public static List<string> ParseReversePolishTokens(this string expression)
        {
            string variablesPattern = @"[a-zA-Z_][a-zA-Z0-9_]*";
            string decimalPattern = @"\d+(\.\d+)?";
            string operatorsPattern = @"[-+*/^]";
            string parenthesesPattern = @"[()]";

            string pattern = $@"{variablesPattern}|{decimalPattern}|{operatorsPattern}|{parenthesesPattern}";

            return new Regex(pattern)
                .Matches(expression)
                .Select(match => match.Value)
                .ToList();
        }

        public static double ResolveUsingReversePolish(this string expression, Dictionary<string, double> variableValues = null)
        {
            var queue = new Queue<string>();
            foreach (var item in expression.Split(' '))
                queue.Enqueue(item);
            return queue.ResolveUsingReversePolish(variableValues);
        }

        public static double ResolveUsingReversePolish(this Queue<string> expression, Dictionary<string, double> variableValues = null)
        {
            variableValues ??= new Dictionary<string, double>();

            if (!(expression?.Any() ?? false))
                throw new NullReferenceException("A expressão não pode ser vazia.");

            var stack = new Stack<double>();

            foreach (var token in expression)
            {
                if (token.IsVariable())
                {
                    if (!variableValues.TryGetValue(token, out double value))
                        throw new ArgumentException($"Não foi possível encontrar o valor da variável \"{token}\".");
                    stack.Push(value);
                }
                else if (token.IsNumber())
                {
                    stack.Push(double.Parse(token, CultureInfo.InvariantCulture));
                }
                else if (token.IsOperator())
                {
                    if (stack.Count < 2)
                        throw new ArgumentException("Não foi possível resolver a expressão, pois ela não está no formato polonês invertido.");

                    var op2 = stack.Pop();
                    var op1 = stack.Pop();

                    if (token.Equals("+"))
                        stack.Push(op1 + op2);
                    else if (token.Equals("-"))
                        stack.Push(op1 - op2);
                    else if (token.Equals("*"))
                        stack.Push(op1 * op2);
                    else if (token.Equals("/"))
                        stack.Push(op1 / op2);
                    else if (token.Equals("^"))
                        stack.Push(Math.Pow(op1, op2));
                    else
                        throw new ArgumentException($"Operação não implementada para o operador: \"{token}\".");
                }
                else
                    throw new ArgumentException($"A expressão contém um elemento inválido: \"{token}\".");
            }

            if (stack.Count != 1)
                throw new ArgumentException("Não foi possível resolver a expressão, pois ela não está no formato polonês invertido.");

            return stack.Pop();
        }

        public static bool IsReversePolish(this string expression)
        {
            var tokens = expression.ParseReversePolishTokens();

            if (!expression.Replace(" ", "").Equals(string.Join("", tokens)))
                return false;

            int count = 0;
            foreach (var token in tokens)
            {
                if (token.IsOperator())
                    count--;
                else if (token.IsNumber() || token.IsVariable())
                    count++;
                else
                    return false;
            }

            return count == 1;
        }
    }
}
