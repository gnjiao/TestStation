using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Array
    {
        public static string ToString<T>(T[][] array)
        {
            string output = Environment.NewLine;

            for (int row = 0; row < array.Length; row++)
            {
                for (int col = 0; col < array[0].Length; col++)
                {
                    output += array[row][col].ToString() + ",";
                }
                output += Environment.NewLine;
            }

            return output;
        }
    }
}