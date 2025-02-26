﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RealeseFinalLZW
{
    public static class ExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> sequance, Action<T> action)
        {
            foreach (T item in sequance)
            {
                action(item);
            }
        }

        public static void WriteLine<T>(this IEnumerable<T> sequence)
        {
            foreach (T item in sequence)
            {
                Console.WriteLine(item);
            }
        }

        public static void WriteLine<K, V>(this Dictionary<K, V> dict)
        {
            foreach (var pair in dict)
            {
                Console.WriteLine("Key: " + pair.Key + ", Value: " + pair.Value);
            }
        }

        public static string ToStringLines<K, V>(this Dictionary<K, V> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in dict)
            {
                sb.Append(("Key: " + pair.Key + ", Value: " + pair.Value));
                sb.AppendLine();
            }

            return sb.ToString();
        }


        public static string FillWithZero(this string value, int len)
        {
            while (value.Length < len)
            {
                value = "0" + value;
            }

            return value;
        }

        public static byte[] ToByteArray(this string value)
        {
            List<byte> l = new List<byte>();

            int i = 0;
            for (i = 0; i < value.Length; i += 8)
            {
                string bs = "";
                if (i + 8 <= value.Length)
                {
                    bs = value.Substring(i, 8);
                }
                else
                {
                    bs = value.Substring(i, value.Length - i);
                }

                byte b = Convert.ToByte(bs, 2);

                l.Add(b);
            }

            return l.ToArray();
        }

        public static string GetBinaryString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            using (BitReader br = new BitReader(bytes))
            {
                bool[] bb = br.ReadAll();
                for (int i = 0; i < bb.Length; i++)
                {
                    bool b = bb[i];
                    sb.Append(Convert.ToInt32(b).ToString());
                }
            }

            return sb.ToString();
        }

        public static string FindKey(this IDictionary<string, int> lookup, int value)
        {
            foreach (var pair in lookup)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }
            }

            return null;
        }

    }
}
