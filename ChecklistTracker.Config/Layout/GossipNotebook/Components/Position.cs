using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Components;

public class Position
{
    public double X { get; set; }

    public double Y { get; set; }
}

internal class ConcretePosition : Position { }

internal class PositionConverter : MultiInputTypeConverter<Position, ConcretePosition>
{
    protected override Position? FromArray(double[] array) => new Position { X = array[1], Y = array[0] };
}
