using System;
using System.Collections.Generic;
using System.Text;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Components;

public class Size
{
    public double Height { get; set; }
    public double Width { get; set; }
}

internal class ConcreteSize : Size { }

internal class SizeConverter : MultiInputTypeConverter<Size, ConcreteSize>
{
    protected override Size? FromArray(double[] array) => new Size { Width = array[1], Height = array[0] };

    protected override Size? FromNumber(double size) => new Size { Width = size, Height = size };
}
