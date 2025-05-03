namespace MicroMLVisualizer.Models.AST
{
    public class IfThenElse : Expression
    {
        public Expression Condition { get; }
        public Expression ThenBranch { get; }
        public Expression ElseBranch { get; }

        public IfThenElse(Expression condition, Expression thenBranch, Expression elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override string ToString()
        {
            return $"if {Condition} then {ThenBranch} else {ElseBranch}";
        }

        public override object ToJson()
        {
            return new
            {
                type = "IfThenElse",
                condition = Condition.ToJson(),
                thenBranch = ThenBranch.ToJson(),
                elseBranch = ElseBranch.ToJson()
            };
        }
    }
}
