namespace MicroMLVisualizer.Models.AST
{
    public abstract class Expression
    {
        public abstract string ToString();
        public abstract object ToJson();
    }
}
