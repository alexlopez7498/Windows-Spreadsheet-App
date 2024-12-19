// <copyright file="ExpressionTree.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SpreadsheetEngine;

namespace SpreadSheetEngine
{
    /// <summary>
    /// Represents an expression tree that can parse mathematical expressions and evaluate them.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// The root node of the expression tree.
        /// </summary>
        private Node? root;

        /// <summary>
        /// A dictionary to store variables and their values.
        /// </summary>
        private Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class, compiling the given expression.
        /// </summary>
        /// <param name="expression">The mathematical expression to be compiled into a tree.</param>
        public ExpressionTree(string expression)
        {
            List<string> postfixExpression = CreatePostfixExpression(expression);
            this.root = this.BuildExpressionTree(postfixExpression);
        }

        /// <summary>
        /// Gets Dictionary of variables.
        /// </summary>
        public Dictionary<string, double> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Converts an infix expression into postfix (RPN) using the Shunting Yard algorithm.
        /// </summary>
        /// <param name="expression">The infix expression.</param>
        /// <returns>The postfix expression.</returns>
        public static List<string> CreatePostfixExpression(string expression)
        {
            Stack<char> stack = new Stack<char>();
            List<string> output = new List<string>();
            string input = string.Empty;

            for (int i = 0; i < expression.Length; i++)
            {
                char token = expression[i];

                // checking if the first character is a letter or digit so we know its not an operator.
                if (char.IsLetterOrDigit(token))
                {
                    input += token;
                }

                // if we know its a ( then we can u just push it and go on to the next token.
                else if (token == '(')
                {
                    stack.Push(token);
                }
                else if (token == '.')
                {
                    input += token;
                }

                // if its a ) then we know that we finished in the ().
                else if (token == ')')
                {
                    if (stack.Peek() != '(')
                    {
                        output.Add(input);
                        input = string.Empty;
                    }

                    // we pop everything in the stack till we reach the ( or end of stack
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        input += stack.Pop();
                        output.Add(input);
                        input = string.Empty;
                    }

                    // then finish off by popping the (
                    stack.Pop();
                }

                // if the token is a operator then we go into here
                else
                {
                    if (input != string.Empty)
                    {
                        output.Add(input);
                        input = string.Empty;
                    }

                    // we pop till the stack is empty and the precedence of the token we have at the moment is less than or equal the top of the stack.
                    while (stack.Count > 0 && OperatorNodeFactory.GetPrecedence(token) <= OperatorNodeFactory.GetPrecedence(stack.Peek()))
                    {
                        input += stack.Pop();
                        output.Add(input);
                        input = string.Empty;
                    }

                    stack.Push(token);
                }
            }

            // we reach the end of expression then we just pop till end of stack.
            while (stack.Count > 0)
            {
                if (input != string.Empty)
                {
                    output.Add(input);
                    input = string.Empty;
                }

                if (stack.Peek() != '(')
                {
                    input += stack.Pop();
                    output.Add(input);
                    input = string.Empty;
                }
                else
                {
                    input += stack.Pop();
                    input = string.Empty;
                }
            }

            if (input != string.Empty)
            {
                output.Add(input);
                input = string.Empty;
            }

            return output;
        }

        /// <summary>
        /// Evaluates the expression tree from its root and returns the result.
        /// </summary>
        /// <returns>The result of evaluating the expression tree.</returns>
        public double Evaluate()
        {
            if (this.root == null)
            {
                return 0;
            }
            else
            {
                return this.Evaluate(this.root);
            }
        }

        /// <summary>
        /// Sets the value of a variable in the expression.
        /// </summary>
        /// <param name="name">The name of the variable to set.</param>
        /// <param name="value">The value to assign to the variable.</param>
        public void SetVariable(string name, double value)
        {
            this.variables[name] = value;
        }

        /// <summary>
        /// Builds the expression tree from the postfix expression.
        /// </summary>
        /// <param name="postfix">The postfix expression.</param>
        /// <returns>The root node of the expression tree.</returns>
        private Node? BuildExpressionTree(List<string> postfix)
        {
            Stack<Node> stack = new Stack<Node>();
            double value = 0;

            // loop through the list of strings
            for (int i = 0; i < postfix.Count; i++)
            {
                string token = postfix[i];

                // making sure its not a operator.
                if (char.IsLetterOrDigit(token[0]) || token[0] == '.')
                {
                    // if we parse the token then we know its a constant node.
                    if (double.TryParse(token, out value))
                    {
                        stack.Push(new ConstantNode(value));
                    }

                    // else then its a variable node.
                    else
                    {
                        if (!this.variables.ContainsKey(token))
                        {
                            this.variables[token] = value;
                        }

                        stack.Push(new VariableNode(this.variables) { Name = token });
                    }
                }

                // its an operator token then we pop left and right and create the operator node.
                else
                {
                    if (stack.Count <= 1)
                    {
                        throw new InvalidExpressionFormula();
                    }

                    Node? right = stack.Pop();
                    Node? left = stack.Pop();

                    OperatorNodeFactory factory = new OperatorNodeFactory();
                    OperatorNode? operatorNode = factory.CreateOperatorNode(token[0]);
                    if (left != null && right != null && operatorNode != null)
                    {
                        operatorNode.Left = left;
                        operatorNode.Right = right;
                    }

                    if (operatorNode != null)
                    {
                        stack.Push(operatorNode);
                    }
                }
            }

            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Evaluates the expression tree recursively, starting from the given node.
        /// </summary>
        /// <param name="node">The node to evaluate.</param>
        /// <returns>The result of evaluating the expression represented by the node.</returns>
        private double Evaluate(Node node)
        {
            return node.Evaluate();
        }
    }
}
