// <copyright file="OperatorNodeFactory.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    /// <summary>
    /// Factory class for creating operator nodes.
    /// Each operator node represents a mathematical operation in an expression.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// Dictionary of operators that holds the operator and the type of node it is.
        /// </summary>
        private Dictionary<char, Type> operators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// calls TraverseAvailableOperators.
        /// </summary>
        public OperatorNodeFactory()
        {
            this.TraverseAvailableOperators((op, type) => this.operators.Add(op, type));
        }

        /// <summary>
        /// Delegate for handling operations defined by an operator and its type.
        /// </summary>
        /// <param name="op">The operator character like +.</param>
        /// <param name="type">The Type associated with the operator.</param>
        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Gets the precedence level of the given operator.
        /// Higher precedence operators are evaluated before lower precedence ones.
        /// </summary>
        /// <param name="op">The operator character ('+', '-', '*', '/').</param>
        /// <returns>
        /// An integer representing the precedence of the operator:.
        /// </returns>
        public static int GetPrecedence(char op)
        {
            OperatorNodeFactory? factory = new OperatorNodeFactory();

            if (op != '(')
            {
                OperatorNode? node = factory.CreateOperatorNode(op);
                if (node != null)
                {
                    return node.Precedence;
                }
            }

            return -1;
        }

        /// <summary>
        /// Creates an operator node based on the given operator character.
        /// </summary>
        /// <param name="op">The operator character ('+', '-', '*', '/').</param>
        /// <returns>An OperatorNode object for the operator, or null if the operator is invalid.</returns>
        public OperatorNode? CreateOperatorNode(char op)
        {
            if (this.operators.ContainsKey(op))
            {
                object? operatorNodeObject = System.Activator.CreateInstance(this.operators[op]);
                if (operatorNodeObject is OperatorNode)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }

            throw new InvalidExpressionFormula();
        }

        /// <summary>
        /// Traverses the available operators and applies the specified action.
        /// </summary>
        /// <param name="onOperator">Delegate to handle each operator and its type.</param>
        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            Type operatorNodeType = typeof(OperatorNode);

            // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                IEnumerable<Type> operatorTypes = assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the Operator property
                    PropertyInfo? operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        // Get the character of the Operator
                        // object value = operatorField.GetValue(type);

                        // If “Operator” property is not static, you will need to create
                        // an instance first and use the following code instead (or similar):
                         object? value = operatorField.GetValue(Activator.CreateInstance(type));
                         if (value is char)
                         {
                            char operatorSymbol = (char)value;

                            // And invoke the function passed as parameter
                            // with the operator symbol and the operator class
                            onOperator(operatorSymbol, type);
                         }
                    }
                }
            }
        }
    }
}
