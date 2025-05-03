namespace MicroMLVisualizer.Models.AST
{
    public class Constant : Expression
    {
        public object Value { get; }
        public string Type { get; }

        public Constant(object value)
        {
            Value = value;
            Type = value is int ? "int" : "bool";
        }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }

        public override object ToJson()
        {
            return new
            {
                type = "Constant",
                nodeType = Type,
                value = Value.ToString().ToLower()
            };
        }
    }
}
