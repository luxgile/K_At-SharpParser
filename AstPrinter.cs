using System.Text;

namespace KatS
{
    public class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expresion) => expresion.Accept(this);

        public string Binary(Binary b)
        {
            return Parenthesize(b.op.Lexeme, b.left, b.right);
        }

        public string Grouping(Grouping g)
        {
            return Parenthesize("group", g.expresion);
        }

        public string Literal(Literal l)
        {
            if (l.value == null) return "undefined";
            return l.value.ToString();
        }

        public string Unary(Unary u)
        {
            return Parenthesize(u.op.Lexeme, u.right);
        }

        private string Parenthesize(string name, params Expr[] expresions)
        {
            StringBuilder sb = new();
            sb.Append('(');
            sb.Append(name);
            for (int i = 0; i < expresions.Length; i++)
            {
                sb.Append(" ").Append(expresions[i].Accept(this));
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}