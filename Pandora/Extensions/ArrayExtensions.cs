using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public static class ArrayExtensions
    {
        public static void Append<T>(ref T[] source, T entity)
        {
            Array.Resize(ref source, source.Length + 1);
            source[source.Length - 1] = entity;
        }

        public static void Prepend<T>(ref T[] source, T entity)
        {
            Array.Reverse(source);
            Append(ref source, entity);
            Array.Reverse(source);
        }
    }
}
