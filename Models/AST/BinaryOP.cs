namespace MicroMLVisualizer.Models.AST
{
    public class BinaryOp : Expression
    {
        public string Operator { get; }
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryOp(string op, Expression left, Expression right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }

        public override object ToJson()
        {
            return new
            {
                type = "BinaryOp",
                op = Operator,
                left = Left.ToJson(),
                right = Right.ToJson()
            };
        }
    }
}
