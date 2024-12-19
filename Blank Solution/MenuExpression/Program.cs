using SpreadSheetEngine;

string? option;
string? expression = "A1+B1+C1";
int optionInt = 0;
int nameValue = 0;
ExpressionTree? exp = null;

//creating Expression tree from default expression
exp = new ExpressionTree(expression);

do
{
    Console.WriteLine("Menu (current expression=(" + expression + "))");
    Console.WriteLine("1 = enter new expression");
    Console.WriteLine("2 = set a variable value");
    Console.WriteLine("3 = Evaulate Tree");
    Console.WriteLine("4 = Quit");
    option = Console.ReadLine();
    // check to see if the user just presses enter
    if(option != "" && option != null)
    {
        optionInt = int.Parse(option);
    }
    // option if they want to enter a new expression
    if (optionInt == 1)
    {
        Console.Write("Enter expression: ");
        expression = Console.ReadLine();
        // new expression tree is created after expression is entered
        if (expression != null)
        {
            exp = new ExpressionTree(expression);
        }

    }
    // option if they want a variable to have a value
    if (optionInt == 2 && exp != null)
    {
        Console.Write("Enter varaible name: ");
        string? name = Console.ReadLine();
        Console.Write("Enter varaible value: ");
        string? value = Console.ReadLine();
        if(value != null)
        {
            nameValue = int.Parse(value);
        }
        if(name != null)
        {
            exp.SetVariable(name, nameValue);
        }
    }
    // option to evaluate the tree
    if (optionInt == 3 && exp != null)
    {

        Console.WriteLine(exp.Evaluate().ToString());
    }
} while (optionInt != 4);
