using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Components;

public class Padding
{
    [JsonInclude]
    public double Left { get; set; }
    [JsonInclude]
    public double Right { get; set; }
    [JsonInclude]
    public double Top { get; set; }
    [JsonInclude]
    public double Bottom { get; set; }

    [JsonInclude]
    internal double Horizontal { set { Left = value; Right = value; } }
    [JsonInclude]
    internal double Vertical { set { Top = value; Bottom = value; } }

    internal Padding() : this(0) { }
    internal Padding(double left, double right, double top, double bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
    internal Padding(double horizontalPadding, double verticalPadding)
        : this(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding) { }

    internal Padding(double uniformSize) : this(uniformSize, uniformSize) { }

    public Thickness ToThickness()
    {
        return new Thickness(
            left: Left,
            top: Top,
            right: Right,
            bottom: Bottom);
    }
}

internal class ConcretePadding : Padding
{
}

internal class PaddingConverter : MultiInputTypeConverter<Padding, ConcretePadding>
{
    protected override Padding? FromArray(double[] value)
    {
        if (value.Length == 1)
        {
            return FromNumber(value[0]);
        }
        if (value.Length == 2)
        {
            return new Padding(value[1], value[0]);
        }
        throw new JsonException($"Unable to parse padding from array of length {value.Length}");
    }
    protected override Padding? FromNumber(double value)
    {
        return new Padding(value);
    }
    protected override Padding? FromString(string value)
    {
        return FromArray(
            value.Replace("px", "")
                 .Split(@" ", StringSplitOptions.RemoveEmptyEntries)
                 .Select(double.Parse)
                 .ToArray());
    }
}
