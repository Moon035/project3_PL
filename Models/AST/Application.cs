namespace MicroMLVisualizer.Models.AST
{
    public class Application : Expression
    {
        public Expression Function { get; }
        public Expression Argument { get; }

        public Application(Expression function, Expression argument)
        {
            Function = function;
            Argument = argument;
        }

        public override string ToString()
        {
            return $"({Function} {Argument})";
        }

        public override object ToJson()
        {
            return new
            {
                type = "Application",
                function = Function.ToJson(),
                argument = Argument.ToJson()
            };
        }
    }
}
