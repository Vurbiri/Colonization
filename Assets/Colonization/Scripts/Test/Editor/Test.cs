using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
	public class Test : MonoBehaviour
	{
        [Button(nameof(Testing))]
        public Color value;

        public void Testing()
        {
            
        }

        public void RosterTest()
        {
            double test1, test2;
            Stopwatch stopWatch = new();
            List<int> list = new();
            Roster<int> set = new();

            TestListAdd(new());

            Thread.Sleep(100);
            stopWatch.Start();
            TestListAdd(list);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestHashAdd(set);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            DrawResultTest(test1, "List", test2, "Roster", "Add");

            Thread.Sleep(100);
            stopWatch.Restart();
            TestListContains(list);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestHashContains(set);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            DrawResultTest(test1, "List", test2, "Roster", "Contains");

            Thread.Sleep(100);
            stopWatch.Restart();
            TestListRemove(list);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestHashRemove(set);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            DrawResultTest(test1, "List", test2, "Roster", "Remove");

            void TestListAdd(List<int> list)
            {
                for(int i = 0; i < 100; ++i)
                    list.Add(i);
            }
            void TestHashAdd(Roster<int> set)
            {
                for (int i = 0; i < 100; ++i)
                    set.Add(i);
            }
            void TestListRemove(List<int> list)
            {
                for (int i = 200; i >= 0; --i)
                    list.Remove(i);
            }
            void TestHashRemove(Roster<int> set)
            {
                for (int i = 200; i >= 0; --i)
                    set.Remove(i);
            }
            void TestListContains(List<int> list)
            {
                for (int i = 200; i >= 0; --i)
                    list.Contains(i);
            }
            void TestHashContains(Roster<int> set)
            {
                for (int i = 200; i >= 0; --i)
                    set.Contains(i);
            }
        }

        public void MathITesting()
        {
            double test1, test2;
            const int count = int.MaxValue >> 10;
            Stopwatch stopWatch = new();

            TestValues(count);

            stopWatch.Start();
            TestMinMath(count);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestMinMathI(count);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            DrawResultTest(test1, "Math", test2, "MathI");

            Thread.Sleep(100);
            stopWatch.Restart();
            TestMinMathI(count);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestMinMath(count);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            DrawResultTest(test1, "Math", test2, "MathI");

            void TestMinMath(int value)
            {
                int x;
                for (int i = -value; i < value; ++i)
                    x = Math.Abs(i);
            }
            void TestMinMathI(int value)
            {
                int x;
                for (int i = -value; i < value; ++i)
                    x = i.Abs();
            }
            void TestValues(int value)
            {
                for (int i = -value; i < value; ++i)
                    if (Math.Abs(i) != i.Abs())
                        Log.Error($"ABS Error [{i}] -> Math{Math.Abs(i)} != MathI{MathI.Abs(i)}");
            }
        }


        private void DrawResultTest(double test1, string name1, double test2, string name2, string title = "=====")
        {
            print($"================= {title} ========================");
            print($"{name1}: {test1}");
            print($"{name2}: {test2}");
            print("-----------------------------------------------");
            print($"{name1}/{name2}: {test1 / test2}");
            print("===============================================");
        }
    }
}
