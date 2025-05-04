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
            // Parse let bindings
            if (_currentToken.Type == TokenType.LET)
                return ParseLetBinding();
            
            // Parse functions
            if (_currentToken.Type == TokenType.FUN)
                return ParseFunction();
            
            // Parse if-then-else
            if (_currentToken.Type == TokenType.IF)
                return ParseIfThenElse();
            
            // Parse comparison expressions
            return ParseComparisonExpression();
        }

        private Expression ParseLetBinding()
        {
            Eat(TokenType.LET);
            
            if (_currentToken.Type != TokenType.IDENTIFIER)
                throw new Exception($"Expected identifier after 'let' at position {_currentToken.Position}");
            
            string varName = _currentToken.Value;
            Eat(TokenType.IDENTIFIER);
            
            Eat(TokenType.EQUALS);
            
            Expression definition = ParseExpression();
            
            Eat(TokenType.IN);
            
            Expression body = ParseExpression();
            
            return new LetBinding(varName, definition, body);
        }

        private Expression ParseFunction()
        {
            Eat(TokenType.FUN);
            
            if (_currentToken.Type != TokenType.IDENTIFIER)
                throw new Exception($"Expected parameter name after 'fun' at position {_currentToken.Position}");
            
            string parameter = _currentToken.Value;
            Eat(TokenType.IDENTIFIER);
            
            Eat(TokenType.ARROW);
            
            Expression body = ParseExpression();
            
            return new Function(parameter, body);
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
            Expression left = ParseApplicationExpression();
            
            while (_currentToken.Type == TokenType.TIMES || _currentToken.Type == TokenType.DIVIDE)
            {
                string op = _currentToken.Value;
                Eat(_currentToken.Type);
                Expression right = ParseApplicationExpression();
                left = new BinaryOp(op, left, right);
            }
            
            return left;
        }

        private Expression ParseApplicationExpression()
        {
            Expression function = ParsePrimaryExpression();
            
            while (_currentToken.Type != TokenType.EOF && 
                   _currentToken.Type != TokenType.RPAREN && 
                   _currentToken.Type != TokenType.THEN && 
                   _currentToken.Type != TokenType.ELSE && 
                   _currentToken.Type != TokenType.IN &&
                   _currentToken.Type != TokenType.PLUS && 
                   _currentToken.Type != TokenType.MINUS && 
                   _currentToken.Type != TokenType.TIMES && 
                   _currentToken.Type != TokenType.DIVIDE && 
                   _currentToken.Type != TokenType.EQUALS && 
                   _currentToken.Type != TokenType.LESS && 
                   _currentToken.Type != TokenType.GREATER)
            {
                Expression argument = ParsePrimaryExpression();
                function = new Application(function, argument);
            }
            
            return function;
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
