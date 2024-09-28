using Antlr4.Runtime;
using System.IO;

namespace ChecklistTracker.ANTLR;

public abstract class Python3ParserBase : Parser
{
    protected Python3ParserBase(ITokenStream input)
        : base(input)
    {
    }

    protected Python3ParserBase(ITokenStream input, TextWriter output, TextWriter errorOutput)
        : base(input, output, errorOutput)
    {
    }

    public bool CannotBePlusMinus()
    {
        return true;
    }

    public bool CannotBeDotLpEq()
    {
        return true;
    }
}
