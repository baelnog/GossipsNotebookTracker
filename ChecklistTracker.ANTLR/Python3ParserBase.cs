using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

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
