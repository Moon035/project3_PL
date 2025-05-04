namespace MicroMLVisualizer.Models.AST
{
    public class Function : Expression
    {
        public string Parameter { get; }
        public Expression Body { get; }

        public Function(string parameter, Expression body)
        {
            Parameter = parameter;
            Body = body;
        }

        public override string ToString()
        {
            return $"fun {Parameter} -> {Body}";
        }

        public override object ToJson()
        {
            return new
            {
                type = "Function",
                parameter = Parameter,
                body = Body.ToJson()
            };
        }
    }
}
