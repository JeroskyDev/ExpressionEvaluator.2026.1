namespace ExpressionEvaluator.Core; //nucleo del evaluador, luego de esto haremos diferentes interfaces de usuario. 

public class Evaluator
{
    //realizar metodo público que nos devuelva un doble, el cual va a ser el resultado de la evaluación, y como no hay que estarlo instanciarlo, lo hacemos estático, y recibe una cadena de texto que es la expresión a evaluar.
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postfix = string.Empty;
        var stack = new Stack<char>();
        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                if(stack.Count == 0)
                {
                    stack.Push(item); //if its an operator and the stack is empty, we push it to the stack
                }
                else
                {
                    //we have to check if the operator is ')'
                    if (item == ')')
                    {
                        do 
                        {
                            postfix += stack.Pop(); //if it is, we pop the stack and add it to the postfix string until we find a '('
                        } while(stack.Peek() != '(');
                        stack.Pop(); //we pop the '(' from the stack but we don't add it to the postfix string
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item); //if the operator has higher priority than the one on top of the stack, we push it to the stack
                        }
                        else
                        {
                            postfix += stack.Pop(); //if the operator has less or equal priority than the one on top of the stack, we pop the stack and add it to the postfix string
                            stack.Push(item); //then we push the current operator to the stack
                        }
                    }
                }
            }
            else
            {
                postfix += item; //if its an operand, we add it to the postfix string
            }
        }

        //if there were operators left in the stack, we pop them and add them to the postfix string
        while (stack.Count > 0)
        {
            postfix += stack.Pop();
        }
        return postfix;
    }

    private static int PriorityStack(char item) => item switch
    {
        '(' => 0,
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        _ => throw new Exception("Syntax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '(' => 5,
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        _ => throw new Exception("Syntax error."),
    };
    

    private static double EvaluatePostfix(string postfix)
    {
        //create a double stack to evaluate the postfix expression
        var stack = new Stack<double>();
        foreach (char item in postfix)
        {
            if (IsOperator(item))
            {
                var b = stack.Pop(); //if its an operator, we pop the last two operands from the stack
                var a = stack.Pop();
                stack.Push(item switch //we apply the operator to the operands (a and b) and push the result back to the stack
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Syntax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(item.ToString())); //if its an operand, we push it to the stack
            }
        }
        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}
