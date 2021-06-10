using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatS
{
    public class Parser
    {
        private Token[] tokens;
        private int current;

        public Parser(Token[] tokens)
        {
            this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
        }

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch
            {
                return null;
            }
        }

        private void Advance() => current++;
        private Token Current() => current < tokens.Length ? tokens[current] : null;
        private Token Previous() => current < tokens.Length ? tokens[current - 1] : null;
        private Token Next() => current < tokens.Length - 1 ? tokens[current + 1] : null;
        private bool Match(params Type[] tokenTypes)
        {
            foreach (Type type in tokenTypes)
            {
                if (current < tokens.Length && Current().GetType() == type)
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Expr Expression()
        {
            return Equality();
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(typeof(IsNotToken), typeof(IsToken)))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Binary
                {
                    left = expr,
                    op = op,
                    right = right,
                };
            }
            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(typeof(GreaterToken), typeof(GreaterOrEqual), typeof(LessToken), typeof(LessOrEqual), typeof(EqualToken)))
            {
                Token op = Previous();
                Expr right = Term();
                expr = new Binary()
                {
                    left = expr,
                    op = op,
                    right = right,
                };
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(typeof(MinusToken), typeof(PlusToken)))
            {
                Token op = Previous();
                Expr right = Factor();
                expr = new Binary()
                {
                    left = expr,
                    op = op,
                    right = right,
                };
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(typeof(StarToken), typeof(SlashToken)))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Binary()
                {
                    left = expr,
                    op = op,
                    right = right,
                };
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(typeof(NotToken), typeof(MinusToken)))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Unary() { op = op, right = right };
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(typeof(FalseToken))) return new Literal() { value = false };
            if (Match(typeof(TrueToken))) return new Literal() { value = true };

            if (Match(typeof(DecToken), typeof(IntToken), typeof(StringToken)))
                return new Literal() { value = Previous().Literal };

            if (Match(typeof(LeftParenToken)))
            {
                Expr expr = Expression();
                if (Current() is not RightParenToken)
                    KatS.ThrowError("Expected ')' after expresion.", Next());
                return new Grouping() { expresion = expr };
            }

            throw KatS.ThrowError("Invalid Primary", Current());
        }

        private void Sync()
        {
            Advance();
            while (current < tokens.Length)
            {
                if (Previous() is SemicolonToken) return;

                switch (Current())
                {
                    case ClassToken:
                    case MethodToken:
                    case IntToken:
                    case DecToken:
                    case IfToken:
                    case RetToken:
                    return;
                }

                Advance();
            }
        }
    }
}
