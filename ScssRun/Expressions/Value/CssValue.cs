﻿using System;
using System.Drawing;
using System.Globalization;

namespace ScssRun.Expressions.Value {
    public struct CssValue {

        public CssValueType Type;

        public double Number;

        public string String;

        public Color Color;

        public override string ToString() {
            switch (Type) {
                case CssValueType.Number:
                    return Number.ToString(CultureInfo.InvariantCulture);
                case CssValueType.Pixel:
                    return Number.ToString(CultureInfo.InvariantCulture) + "px";
                case CssValueType.Em:
                    return Number.ToString(CultureInfo.InvariantCulture) + "em";
                case CssValueType.Rem:
                    return Number.ToString(CultureInfo.InvariantCulture) + "rem";
                case CssValueType.Percentage:
                    return Number.ToString(CultureInfo.InvariantCulture) + "%";
                case CssValueType.Inch:
                    return Number.ToString(CultureInfo.InvariantCulture) + "inch";
                case CssValueType.ViewportWidth:
                    return Number.ToString(CultureInfo.InvariantCulture) + "vw";
                case CssValueType.ViewportHeight:
                    return Number.ToString(CultureInfo.InvariantCulture) + "vh";
                case CssValueType.String:
                    return String;
                default:
                    throw new Exception("unknon value type");
            }
        }
    }
}