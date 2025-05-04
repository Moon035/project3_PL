namespace MicroMLVisualizer.Models.AST
{
    public class LetBinding : Expression
    {
        public string Variable { get; }
        public Expression Definition { get; }
        public Expression Body { get; }

        public LetBinding(string variable, Expression definition, Expression body)
        {
            Variable = variable;
            Definition = definition;
            Body = body;
        }

        public override string ToString()
        {
            return $"let {Variable} = {Definition} in {Body}";
        }

        public override object ToJson()
        {
            return new
            {
                type = "LetBinding",
                variable = Variable,
                definition = Definition.ToJson(),
                body = Body.ToJson()
            };
        }
    }
}
