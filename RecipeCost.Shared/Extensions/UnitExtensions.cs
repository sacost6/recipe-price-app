using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecipeCost.Shared.Extensions
{
    public static class UnitExtensions
    {
        public static UnitType GetBaseUnit(this UnitType unit)
        {
            var field = unit.GetType().GetField(unit.ToString());
            var attribute = field?.GetCustomAttribute<CategoryAttribute>();

            return attribute?.Category switch
            {
                "Mass" => UnitType.Gram,
                "Volume" => UnitType.Milliliter,
                "Discrete" => UnitType.Each,
                _ => throw new ArgumentException($"Unit {unit} is uncategorized.")
            };
        }
    }
}
