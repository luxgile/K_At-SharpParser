using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KatS
{
    internal class Scanner
    {
        private string source;
        private List<Token> keywords = new();

        public Scanner(string source)
        {
            this.source = source;
            keywords.Add(new OrToken());
            keywords.Add(new AndToken());
            keywords.Add(new GreaterToken());
            keywords.Add(new LessToken());
            keywords.Add(new NotToken());
            keywords.Add(new IsToken());
            keywords.Add(new MethodToken());
            keywords.Add(new ClassToken());
            keywords.Add(new IfToken());
        }

        public List<Token> ScanTokens()
        {
            List<Token> tokens = new();
            int tokenStart = 0;
            int tokenCurrent = 0;
            int line = 1;
            while (tokenCurrent < source.Length)
            {
                tokenStart = tokenCurrent;
                ScanToken(tokenStart, ref tokenCurrent, ref line, tokens);
            }
            tokens.Add(new EOFToken());
            return tokens;
        }

        private char GetChar(int index)
        {
            if (index >= source.Length) return '\0';
            return source[index];
        }

        private void ScanToken(int tokenStart, ref int tokenCurrent, ref int line, List<Token> tokens)
        {
            char c = GetChar(tokenCurrent++);
            switch (c)
            {
                case '(': tokens.Add(new LeftParenToken()); break;
                case ')': tokens.Add(new RightParenToken()); break;
                case '{': tokens.Add(new LeftBraceToken()); break;
                case '}': tokens.Add(new RightBraceToken()); break;
                case ',': tokens.Add(new CommaToken()); break;
                case '.': tokens.Add(new DotToken()); break;
                case ';': tokens.Add(new SemicolonToken()); break;
                case '+': tokens.Add(new PlusToken()); break;
                case '-': tokens.Add(new MinusToken()); break;
                case '*': tokens.Add(new StarToken()); break;
                case '/':
                if (GetChar(tokenCurrent) == '/')
                {
                    //It's a comment.
                    while (GetChar(tokenCurrent) is not '\n') { tokenCurrent++; }
                }
                else
                    tokens.Add(new SlashToken());
                break;

                case '"':
                while (GetChar(tokenCurrent) != '"')
                {
                    if (GetChar(tokenCurrent) == '\n') line++;
                    tokenCurrent++;
                }
                if (tokenCurrent == source.Length)
                {
                    KatS.ThrowError("String not terminated", "", line);
                    return;
                }

                tokenCurrent++;

                string value = source[(tokenStart + 1)..(tokenCurrent - 1)];
                tokens.Add(new StringToken() { Literal = value });
                break;

                case ' ':
                case '\t':
                case '\r':
                break;

                case '\n':
                line++;
                break;

                default:
                if (char.IsDigit(c))
                {
                    while (char.IsDigit(GetChar(tokenCurrent))) tokenCurrent++;

                    if(GetChar(tokenCurrent) == '.' && char.IsDigit(GetChar(tokenCurrent + 1)))
                    {
                        tokenCurrent++;
                        while (char.IsDigit(GetChar(tokenCurrent))) tokenCurrent++;
                        string sub = source[tokenStart..tokenCurrent];
                        tokens.Add(new DecToken() { Literal = double.Parse(sub, CultureInfo.InvariantCulture) });
                    }
                    else
                    {
                        string sub = source[tokenStart..tokenCurrent];
                        tokens.Add(new IntToken() { Literal = int.Parse(sub) });
                    }
                }
                else if(char.IsLetter(c))
                {
                    while (char.IsLetter(GetChar(tokenCurrent))) tokenCurrent++;
                    int curr = tokenCurrent;
                    string identifier = source[tokenStart..curr];
                    Token token = keywords.Where(x => x.Lexeme == identifier).FirstOrDefault();
                    if (token != null)
                        tokens.Add(token);
                }
                else
                {
                    KatS.ThrowError($"Unidentified character: {c}", "", line);
                }
                break;
            }
        }
    }
}