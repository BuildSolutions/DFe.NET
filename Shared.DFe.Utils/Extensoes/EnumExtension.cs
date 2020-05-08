using System;
using System.Collections.Generic;
using System.Text;

namespace DFe.Utils.Extensoes
{
    public static class EnumExtension
    {
        public static T? NuloSeInvalido<T>(this T? valor) where T: struct, IConvertible
        {
            var enumType = typeof(T);
            if(!enumType.IsEnum
                || valor == null)
            {
                return null;
            }

            if (valor.GetValueOrDefault().EValido())
            {
                return (T)Enum.Parse(enumType, valor.ToString());
            }
            return null;
        }

        public static T NuloSeInvalido<T>(this T valor) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return default(T);
            }

            if (valor.EValido())
            {
                return (T)Enum.Parse(enumType, valor.ToString());
            }
            return default(T);
        }

        public static bool EValido<T>(this T valor) where T : struct, IConvertible
        {
            return Enum.IsDefined(typeof(T), valor);
        }
    }
}
