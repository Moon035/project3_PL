namespace MicroMLVisualizer.Models.AST
{
    public class Variable : Expression
    {
        public string Name { get; }

        public Variable(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override object ToJson()
        {
            return new
            {
                type = "Variable",
                name = Name
            };
        }
    }
}
