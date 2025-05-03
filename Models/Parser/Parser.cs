using System;
using System.Collections.Generic;
using MicroMLVisualizer.Models.AST;

namespace MicroMLVisualizer.Models.Parser
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _position;
        private Token _currentToken;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _currentToken = _tokens[_position];
        }

        private void Advance()
        {
            _position++;
            if (_position < _tokens.Count)
                _currentToken = _tokens[_position];
        }

        private void Eat(TokenType tokenType)
        {
            if (_currentToken.Type == tokenType)
                Advance();
            else
                throw new Exception($"Expected {tokenType} but got {_currentToken.Type} at position {_currentToken.Position}");
        }

        public Expression Parse()
        {
            Expression result = ParseExpression();
            
            if (_currentToken.Type != TokenType.EOF)
                throw new Exception($"Unexpected token: {_currentToken.Type} at position {_currentToken.Position}");
            
            return result;
        }

        private Expression ParseExpression()
        {
            if (_currentToken.Type == TokenType.IF)
                return ParseIfThenElse();
            
            return ParseComparisonExpression();
        }

        private Expression ParseIfThenElse()
        {
            Eat(TokenType.IF);
            Expression condition = ParseExpression();
            Eat(TokenType.THEN);
            Expression thenBranch = ParseExpression();
            Eat(TokenType.ELSE);
            Expression elseBranch = ParseExpression();
            
            return new IfThenElse(condition, thenBranch, elseBranch);
        }

        private Expression ParseComparisonExpression()
        {
            Expression left = ParseAdditiveExpression();
            
            while (_currentToken.Type == TokenType.EQUALS || 
                   _currentToken.Type == TokenType.LESS || 
                   _currentToken.Type == TokenType.GREATER)
            {
                string op = _currentToken.Value;
                Eat(_currentToken.Type);
                Expression right = ParseAdditiveExpression();
                left = new BinaryOp(op, left, right);
            }
            
            return left;
        }

        private Expression ParseAdditiveExpression()
        {
            Expression left = ParseMultiplicativeExpression();
            
            while (_currentToken.Type == TokenType.PLUS || _currentToken.Type == TokenType.MINUS)
            {
                string op = _currentToken.Value;
                Eat(_currentToken.Type);
                Expression right = ParseMultiplicativeExpression();
                left = new BinaryOp(op, left, right);
            }
            
            return left;
        }

        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParsePrimaryExpression();
            
            while (_currentToken.Type == TokenType.TIMES || _currentToken.Type == TokenType.DIVIDE)
            {
                string op = _currentToken.Value;
                Eat(_currentToken.Type);
                Expression right = ParsePrimaryExpression();
                left = new BinaryOp(op, left, right);
            }
            
            return left;
        }

        private Expression ParsePrimaryExpression()
        {
            Token token = _currentToken;
            
            switch (token.Type)
            {
                case TokenType.INTEGER:
                    Eat(TokenType.INTEGER);
                    return new Constant(int.Parse(token.Value));
                
                case TokenType.BOOLEAN:
                    Eat(TokenType.BOOLEAN);
                    return new Constant(bool.Parse(token.Value.ToLower()));
                
                case TokenType.IDENTIFIER:
                    Eat(TokenType.IDENTIFIER);
                    return new Variable(token.Value);
                
                case TokenType.LPAREN:
                    Eat(TokenType.LPAREN);
                    Expression result = ParseExpression();
                    Eat(TokenType.RPAREN);
                    return result;
                
                default:
                    throw new Exception($"Unexpected token: {token.Type} at position {token.Position}");
            }
        }
    }
}
