using System.Collections.Generic;

namespace KatS
{
    public abstract class Token
    {
        public List<Token> childs = new();
        public abstract string Lexeme { get; }
        public virtual object Literal { get; set; }
        public int Line { get; set; }
    }

    public class LeftParenToken : Token
    {
        public override string Lexeme => "(";
    }

    public class RightParenToken : Token
    {
        public override string Lexeme => ")";
    }

    public class LeftBraceToken : Token
    {
        public override string Lexeme => "{";
    }

    public class RightBraceToken : Token
    {
        public override string Lexeme => "}";
    }

    public class CommaToken : Token
    {
        public override string Lexeme => ",";
    }

    public class DotToken : Token
    {
        public override string Lexeme => ".";
    }

    public class MinusToken : Token
    {
        public override string Lexeme => "-";
    }

    public class PlusToken : Token
    {
        public override string Lexeme => "+";
    }

    public class SemicolonToken : Token
    {
        public override string Lexeme => ";";
    }

    public class SlashToken : Token
    {
        public override string Lexeme => "/";
    }

    public class StarToken : Token
    {
        public override string Lexeme => "*";
    }
    public class ThisToken : Token
    {
        public override string Lexeme => "this";
    }

    public class RetToken : Token
    {
        public override string Lexeme => "ret";
    }

    public class AndToken : Token
    {
        public override string Lexeme => "and";
    }

    public class EqualToken : Token
    {
        public override string Lexeme => "equal";
    }

    public class GreaterToken : Token
    {
        public override string Lexeme => "great";
    }

    public class LessToken : Token
    {
        public override string Lexeme => "less";
    }

    public class OrToken : Token
    {
        public override string Lexeme => "or";
    }

    public class GreaterOrEqual : Token
    {
        public override string Lexeme => "great or equal";
    }

    public class LessOrEqual : Token
    {
        public override string Lexeme => "less or equal";
    }

    public class NotToken : Token
    {
        public override string Lexeme => "not";
    }
    public class IsToken : Token
    {
        public override string Lexeme => "is";
    }
    public class IsNotToken : Token
    {
        public override string Lexeme => "is not";
    }

    public class IdentifierToken : Token
    {
        public override string Lexeme => Literal.ToString();
    }

    public class StringToken : Token
    {
        public override string Lexeme => Literal.ToString();
    }

    public class IntToken : Token
    {
        public override string Lexeme => Literal.ToString();
    }

    public class DecToken : Token
    {
        public override string Lexeme => Literal.ToString();
    }

    public class MethodToken : Token
    {
        public override string Lexeme => "method";
    }

    public class ClassToken : Token
    {
        public override string Lexeme => "class";
    }

    public class IfToken : Token
    {
        public override string Lexeme => "if";
    }

    public class TrueToken : Token
    {
        public override string Lexeme => "true";
    }

    public class FalseToken : Token
    {
        public override string Lexeme => "false";
    }

    public class EOFToken : Token
    {
        public override string Lexeme => "";
    }
}
