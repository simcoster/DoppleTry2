﻿using Dopple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestedFunctions
{
    public class InProgress
    {
        //public static int TestConditionals(int a, int b)
        //{
        //    Helper helper = new Helper();
        //    if (a==b)
        //    {
        //        helper.Number = 4;
        //    }
        //    else
        //    {
        //        helper.Number = 1;
        //    }
        //    helper.Number = 5;
        //    helper.Text = "blah";
        //    return helper.Number;
        //}
        //public static List<int> UseDelegate(int[] nums)
        //{
        //    return nums.Select(x => x++).ToList();
        //}
        public static int testLinq(int[] nums)
        {
            int sum = 0;
            foreach (var num in nums.Where(x => x > 6))
            {
                sum += num;
            }
            return sum;
        }
        //public static int SumMe(int[] nums)
        //{
        //    return nums.Sum(x => x * 6);
        //}

        //public static int DynamicGames(int index)
        //{
        //    int a = 5;
        //    Helper helper = new Helper();
        //    while (a > 6)
        //    {
        //        a++;
        //        helper.Number = a;
        //        a = helper.Number;
        //    }
        //    return a;
        //}
    }

    public class Helper
    {
        public int Number;
        public string Text;
        public string Text2;
        public Helper()
        {
            //Number = 4;
            //Text2 = "Blaaa";
        }
        //neeed to add a real exmaple
        //object with object field in it and with 2 inheriting implementing 2 a virtual method
    }
}