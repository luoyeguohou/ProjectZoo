using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class ExpressionParser
{
    private static int GetPrecedence(char op)
    {
        return op switch
        {
            '+' or '-' => 1,
            '*' or '/' => 2,
            _ => 0
        };
    }

    private static bool IsOperator(char c) => "+-*/".Contains(c);

    public static List<string> ToPostfix(string expr)
    {
        var output = new List<string>();
        var opStack = new Stack<char>();
        var number = new StringBuilder();

        for (int i = 0; i < expr.Length; i++)
        {
            char c = expr[i];

            if (char.IsDigit(c))
            {
                number.Append(c);
            }
            else
            {
                if (number.Length > 0)
                {
                    output.Add(number.ToString());
                    number.Clear();
                }

                if (c == ' ') continue;

                if (c == '(')
                {
                    opStack.Push(c);
                }
                else if (c == ')')
                {
                    while (opStack.Count > 0 && opStack.Peek() != '(')
                        output.Add(opStack.Pop().ToString());
                    if (opStack.Count == 0 || opStack.Pop() != '(')
                        throw new Exception("Mismatched parentheses");
                }
                else if (IsOperator(c))
                {
                    while (opStack.Count > 0 && IsOperator(opStack.Peek()) &&
                           GetPrecedence(opStack.Peek()) >= GetPrecedence(c))
                    {
                        output.Add(opStack.Pop().ToString());
                    }
                    opStack.Push(c);
                }
                else
                {
                    throw new Exception($"Invalid character: {c}");
                }
            }
        }

        if (number.Length > 0)
            output.Add(number.ToString());

        while (opStack.Count > 0)
        {
            char op = opStack.Pop();
            if (op == '(' || op == ')')
                throw new Exception("Mismatched parentheses");
            output.Add(op.ToString());
        }

        return output;
    }
}

public class PostfixEvaluator
{
    public static int Evaluate(string s)
    {
        List<string> tokens = ExpressionParser.ToPostfix(s);
        var stack = new Stack<int>();

        foreach (var token in tokens)
        {
            if (int.TryParse(token, out int num))
            {
                stack.Push(num);
            }
            else
            {
                int b = stack.Pop();
                int a = stack.Pop();

                int result = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    _ => throw new Exception($"Invalid operator: {token}")
                };

                stack.Push(result);
            }
        }

        return stack.Pop();
    }
}
