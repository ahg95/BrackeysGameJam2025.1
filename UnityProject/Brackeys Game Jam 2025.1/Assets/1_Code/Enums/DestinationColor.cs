using System.Collections.Generic;
using UnityEngine;

namespace _1_Code.Enums
{
    public enum DestinationColor
    {
        Blue,
        Green,
        Yellow,
        Red,
        Orange,
        Purple,
        Pink,
        Brown,
        Cyan,
        Magenta
    }

    public static class DestinationColorExtensions
    {
        private static readonly Dictionary<DestinationColor, Color> DestinationColorMap =
            new Dictionary<DestinationColor, Color>
            {
                { DestinationColor.Blue, Color.blue },
                { DestinationColor.Green, Color.green },
                { DestinationColor.Yellow, Color.yellow },
                { DestinationColor.Red, Color.red },
                { DestinationColor.Orange, new Color(1.0f, 0.5f, 0.0f) },
                { DestinationColor.Purple, new Color(0.5f, 0.0f, 0.5f) },
                { DestinationColor.Pink, new Color(1.0f, 0.75f, 0.8f) },
                { DestinationColor.Brown, new Color(0.6f, 0.3f, 0.1f) },
                { DestinationColor.Cyan, Color.cyan },
                { DestinationColor.Magenta, Color.magenta }
            };

        public static Color GetColor(this DestinationColor destinationColor) => DestinationColorMap[destinationColor];
    }
}