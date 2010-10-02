using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace System.Windows
{
    static class ExtensionMethods
    {
        public static void DisposeObject<T>(ref T disposable) where T : class, IDisposable
        {
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }

        public static void BinaryInsert<T>(this List<T> list, T value) where T : IComparable<T>
        {
            int start = 0;
            int end = list.Count;

            while (start < end)
            {
                int len = end - start;
                int middle = start + (len / 2);
                int comp = value.CompareTo(list[middle]);
                if (comp == 0)
                {
                    start = middle;
                    break;
                }
                else if (comp < 0)
                {
                    end = middle;
                }
                else
                {
                    start = middle + (len % 2);
                }
            }
            list.Insert(start, value);
        }

        public static void BinaryInsert<T>(this List<T> list, T value, IComparer<T> comparer)
        {
            int start = 0;
            int end = list.Count;

            while (start < end)
            {
                int len = end - start;
                int middle = start + (len / 2);
                int comp = comparer.Compare(value, list[middle]);
                if (comp == 0)
                {
                    start = middle;
                    break;
                }
                else if (comp < 0)
                {
                    end = middle;
                }
                else
                {
                    start = middle + (len % 2);
                }
            }
            list.Insert(start, value);
        }

        public static void BinaryInsert<T>(this List<T> list, T value, Comparison<T> comparison)
        {
            int start = 0;
            int end = list.Count;

            while (start < end)
            {
                int len = end - start;
                int middle = start + (len / 2);
                int comp = comparison(value, list[middle]);
                if (comp == 0)
                {
                    start = middle;
                    break;
                }
                else if (comp < 0)
                {
                    end = middle;
                }
                else
                {
                    start = middle + (len % 2);
                }
            }
            list.Insert(start, value);
        }
    }
}
