﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy.Extensions
{
    public static class BitConverterExtension
    {
        public static byte[] GetBytes(this string data)
        {
            var length = (data.Length + 1) / 3;
            var array = new byte[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = Convert.ToByte(data.Substring(3 * i, 2), 16);
            }
            return array;
        }
    }
}