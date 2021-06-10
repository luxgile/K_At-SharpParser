namespace KatS
{
    public interface IVisitor<T>
    {
        T Binary(Binary b);
        T Grouping(Grouping g);
        T Literal(Literal l);
        T Unary(Unary u);
    }

    public abstract class Expr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }

    public class Binary : Expr
    {
        public Expr left;
        public Token op;
        public Expr right;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.Binary(this);
    }

    public class Grouping : Expr
    {
        public Expr expresion;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.Grouping(this);
    }

    public class Literal : Expr
    {
        public object value;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.Literal(this);
    }

    public class Unary : Expr
    {
        public Token op;
        public Expr right;

        public override T Accept<T>(IVisitor<T> visitor) => visitor.Unary(this);
    }
}