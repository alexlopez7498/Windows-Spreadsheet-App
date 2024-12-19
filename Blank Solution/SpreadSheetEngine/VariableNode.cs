// <copyright file="VariableNode.cs" company="Alex Lopez-Garcia">
// Copyright (c) Alex Lopez-Garcia. All rights reserved.
// </copyright>

namespace SpreadSheetEngine
{
    /// <summary>
    /// Variable Node which just hold the name of a variable.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// A dictionary to store variables and their values.
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="variables">the dictionary of the variables.</param>
        public VariableNode(Dictionary<string, double> variables)
        {
            this.variables = variables;
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets for name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// override evaluate method which just gets the value of the variable.
        /// </summary>
        /// <returns>the value of the variable.</returns>
        public override double Evaluate()
        {
            return this.variables[this.Name];
        }
    }
}
