// <copyright file="ExpressionTreeTests.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetEngine;
using SpreadSheetEngine;

namespace SpreadSheetTests
{
    /// <summary>
    /// public class ExpressionTreeTest that has test method for the project.
    /// </summary>
    public class ExpressionTreeTests
    {
        /// <summary>
        /// Tests if the Evaluate method actually returns the correct sum of the 3 numbers.
        /// </summary>
        [Test]
        public void EvaluateNormalExpression()
        {
            double result = 0;
            string expression = "42+12+1";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(55));
        }

        /// <summary>
        /// Tests if the Evaluate method actually returns the correct sum of the 3 numbers with two being variable
        /// nodes that have set values.
        /// </summary>
        [Test]
        public void EvaluateExpressionWithVariables()
        {
            double result = 0;
            string expression = "42+hi+hello";
            ExpressionTree tree = new ExpressionTree(expression);
            tree.SetVariable("hi", 21);
            tree.SetVariable("hello", 7);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(70));
        }

        /// <summary>
        /// Tests if the Evaluate method actually returns the correct negative number from the two numbers.
        /// </summary>
        [Test]
        public void EvaluateExpressionWithAnswerNegative()
        {
            double result = 0;
            string expression = "100-1000";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(-900));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct product of two numbers.
        /// </summary>
        [Test]
        public void EvaluateMultiplyExpression()
        {
            double result = 0;
            string expression = "7*6";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(42));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a division expression.
        /// </summary>
        [Test]
        public void EvaluateEmptyExpression()
        {
            double result = 0;
            string expression = string.Empty;
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a division expression.
        /// </summary>
        [Test]
        public void EvaluateSingleNodeExpression()
        {
            double result = 0;
            string expression = "84";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(84));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a add and subtract expression.
        /// </summary>
        [Test]
        public void EvaluateAddAndSubtractExpression()
        {
            double result = 0;
            string expression = "8+4-2";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(10));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a multiply, add, and subtract expression.
        /// </summary>
        [Test]
        public void EvaluateMultiplyAndAddExpression()
        {
            double result = 0;
            string expression = "4+8*(8-2)";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(52));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a ton of parentheses expression.
        /// </summary>
        [Test]
        public void EvaluateLotsOfParenthesesExpression()
        {
            double result = 0;
            string expression = "((((5+4))))";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(9));
        }

        /// <summary>
        /// Tests if the Evaluate method returns the correct result of a division by a zero expression.
        /// </summary>
        [Test]
        public void EvaluateDivideZeroExpression()
        {
            double result = 0;
            string expression = "8/0";
            ExpressionTree tree = new ExpressionTree(expression);
            result = tree.Evaluate();
            Assert.That(result, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a simple adding expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionAdding()
        {
            string infixExpression = "3+5";
            List<string> expectedPostfix = new List<string> { "3", "5", "+" };
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a parentheses and without expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionWithParentheses()
        {
            string infixExpression = "(3+5)*2";
            List<string> expectedPostfix = new List<string> { "3", "5", "+", "2", "*" };
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a single ( expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionSingleParenthesis()
        {
            string infixExpression = "(";
            List<string> expectedPostfix = new List<string>();
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a adding and multiplying expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionWithDifferentPrecedence()
        {
            string infixExpression = "3*5+2";
            List<string> expectedPostfix = new List<string> { "3", "5", "*", "2", "+" };
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a bit more complex expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionWithNestedParentheses()
        {
            string infixExpression = "((3+5)*2)/4";
            List<string> expectedPostfix = new List<string> { "3", "5", "+", "2", "*", "4", "/" };
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a empty expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionEmptyString()
        {
            string infixExpression = string.Empty;
            List<string> expectedPostfix = new List<string>();
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }

        /// <summary>
        /// Tests if the CreatPostFix method creates the correct order in postfix with a complex expression.
        /// </summary>
        [Test]
        public void TestCreatePostfixExpressionWithComplexOperators()
        {
            string infixExpression = "3+(4*5-6)/7";
            List<string> expectedPostfix = new List<string> { "3", "4", "5", "*", "6", "-", "7", "/", "+" };
            List<string> actualPostfix = ExpressionTree.CreatePostfixExpression(infixExpression);
            Assert.That(actualPostfix, Is.EqualTo(expectedPostfix));
        }
    }
}