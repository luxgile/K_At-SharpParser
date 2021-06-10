using System;
using System.Collections.Generic;

namespace KatS
{
    public class ScannerGrouper
    {
        private List<Token> tokens;
        private List<Token> result = new();

        public ScannerGrouper(List<Token> tokens)
        {
            this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
        }

        public List<Token> GroupTokens()
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                switch (tokens[i])
                {
                    case IsToken isToken:
                    if (tokens[i + 1] is NotToken notToken)
                    {
                        result.Add(new IsNotToken() { childs = { isToken, notToken } });
                        i++;
                    }
                    else
                        result.Add(isToken);
                    break;

                    case GreaterToken greatToken:
                    {
                        if (tokens[i + 1] is OrToken orToken && tokens[i + 2] is EqualToken equalsToken)
                        {
                            result.Add(new GreaterOrEqual() { childs = { greatToken, orToken, equalsToken } });
                            i += 2;
                        }
                        else
                            result.Add(greatToken);
                    }
                    break;

                    case LessToken lessToken:
                    {
                        if (tokens[i + 1] is OrToken orToken && tokens[i + 2] is EqualToken equalsToken)
                        {
                            result.Add(new LessOrEqual() { childs = { lessToken, orToken, equalsToken } });
                            i += 2;
                        }
                        else
                            result.Add(lessToken);
                    }
                    break;

                    default:
                    result.Add(tokens[i]);
                    break;
                }
            }

            return result;
        }
    }
}
