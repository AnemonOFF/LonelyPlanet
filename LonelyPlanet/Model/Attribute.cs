using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public static class Attributes
    {
        public static readonly Attribute Player = new Attribute(new Dictionary<string, double> { {"MaxHealth", 100}, {"MaxOxygen", 100}, { "MaxHotBarAmount", 6}, { "MaxSolidAmount", 24}, { "MaxLiquidAmount", 10000} });
    }

    public class Attribute
    {
        private readonly Dictionary<string, double> fields;

        public Attribute(Dictionary<string, double> fields)
        {
            this.fields = fields.ToDictionary(x=>x.Key, x=>x.Value);
        }

        public double Get(string field)
        {
            return fields[field];
        }
    }
}
