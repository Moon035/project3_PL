namespace MicroMLVisualizer.Models.Parser
{
    public enum TokenType
    {
        INTEGER, BOOLEAN, IDENTIFIER, LPAREN, RPAREN,
        PLUS, MINUS, TIMES, DIVIDE, EQUALS, LESS, GREATER,
        IF, THEN, ELSE, FUN, ARROW, LET, IN, EOF
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Position { get; }

        public Token(TokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return $"{Type}: '{Value}' at position {Position}";
        }
    }
}
