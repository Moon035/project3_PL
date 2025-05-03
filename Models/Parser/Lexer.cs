using System;
using System.Collections.Generic;

namespace MicroMLVisualizer.Models.Parser
{
    public class Lexer
    {
        private string _input;
        private int _position;
        private char _currentChar;

        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            { "if", TokenType.IF },
            { "then", TokenType.THEN },
            { "else", TokenType.ELSE },
            { "fun", TokenType.FUN },
            { "true", TokenType.BOOLEAN },
            { "false", TokenType.BOOLEAN },
            { "let", TokenType.LET },
            { "in", TokenType.IN }
        };

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
            if (_input.Length > 0)
                _currentChar = _input[_position];
            else
                _currentChar = '\0';
        }

        private void Advance()
        {
            _position++;
            if (_position < _input.Length)
                _currentChar = _input[_position];
            else
                _currentChar = '\0';
        }

        private char Peek()
        {
            int peekPos = _position + 1;
            if (peekPos >= _input.Length)
                return '\0';
            return _input[peekPos];
        }

        private void SkipWhitespace()
        {
            while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
                Advance();
        }

        private Token Number()
        {
            string result = "";
            int startPos = _position;

            while (_currentChar != '\0' && char.IsDigit(_currentChar))
            {
                result += _currentChar;
                Advance();
            }

            return new Token(TokenType.INTEGER, result, startPos);
        }

        private Token Identifier()
        {
            string result = "";
            int startPos = _position;

            while (_currentChar != '\0' && (char.IsLetterOrDigit(_currentChar) || _currentChar == '_'))
            {
                result += _currentChar;
                Advance();
            }

            if (_keywords.ContainsKey(result))
                return new Token(_keywords[result], result, startPos);
            else
                return new Token(TokenType.IDENTIFIER, result, startPos);
        }

        public Token GetNextToken()
        {
            while (_currentChar != '\0')
            {
                if (char.IsWhiteSpace(_currentChar))
                {
                    SkipWhitespace();
                    continue;
                }

                if (char.IsDigit(_currentChar))
                    return Number();

                if (char.IsLetter(_currentChar) || _currentChar == '_')
                    return Identifier();

                int currentPos = _position;
                switch (_currentChar)
                {
                    case '(':
                        Advance();
                        return new Token(TokenType.LPAREN, "(", currentPos);
                    case ')':
                        Advance();
                        return new Token(TokenType.RPAREN, ")", currentPos);
                    case '+':
                        Advance();
                        return new Token(TokenType.PLUS, "+", currentPos);
                    case '-':
                        if (Peek() == '>')
                        {
                            Advance();
                            Advance();
                            return new Token(TokenType.ARROW, "->", currentPos);
                        }
                        Advance();
                        return new Token(TokenType.MINUS, "-", currentPos);
                    case '*':
                        Advance();
                        return new Token(TokenType.TIMES, "*", currentPos);
                    case '/':
                        Advance();
                        return new Token(TokenType.DIVIDE, "/", currentPos);
                    case '=':
                        Advance();
                        return new Token(TokenType.EQUALS, "=", currentPos);
                    case '<':
                        Advance();
                        return new Token(TokenType.LESS, "<", currentPos);
                    case '>':
                        Advance();
                        return new Token(TokenType.GREATER, ">", currentPos);
                    default:
                        throw new Exception($"Unexpected character: '{_currentChar}' at position {_position}");
                }
            }

            return new Token(TokenType.EOF, "", _position);
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            
            while (token.Type != TokenType.EOF)
            {
                tokens.Add(token);
                token = GetNextToken();
            }
            
            tokens.Add(token);
            return tokens;
        }
    }
}
