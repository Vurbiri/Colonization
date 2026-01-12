using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class Test : MonoBehaviour
	{
		[Button(nameof(Testing))]
		public Color value;

		public void Testing()
		{
			var arr = new Q[] { new() };
			print(arr[0].x);
			arr[0].Add(5);
            print(arr[0].x);

			List<Q> list = new() { new() };
            print(list[0].x);
            list[0].Add(5);
            print(list[0].x);
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

		private struct Q
		{
			public int x;

			public void Add(int i) => x += i;
		}
	}
}
