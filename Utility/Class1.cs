﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utility
{
    public class Class1
    {
        static public int AddRec(int[] arr, int toSearch, int startIndex)
        {
            if (arr[startIndex] == toSearch)
            {
                return startIndex;
            }
            else
            {
                var a =  AddRec(arr, toSearch, toSearch + 1);
                a = toSearch;
                return a;
            }
        }

        //static public int AddLoop(int a, int b)
        //{
        //    while (a + b < 5)
        //    {
        //        a += 2;
        //        b += 1;
        //    }
        //    return a + b;
        //}
    }
}
