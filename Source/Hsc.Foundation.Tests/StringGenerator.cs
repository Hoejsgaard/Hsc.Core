﻿using System;
using System.Text;

namespace Hsc.Foundation.Tests
{
    public class StringGenerator
    {
        public static string BuildRandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random(315341354);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}