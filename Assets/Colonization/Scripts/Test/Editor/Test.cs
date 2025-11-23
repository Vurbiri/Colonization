using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class Test : MonoBehaviour
	{
        [Space]
        public FileIdAndKey giftMsg;
        [Button(nameof(Testing))]
        public Color value;
        [Button(nameof(Testing2), false)]
        public SkillApplied skillApplied;

        private TMP_Dropdown _dropdown;

 

        private void Start()
        {
            _dropdown = GetComponentInChildren<TMP_Dropdown>();

            _dropdown.ClearOptions();
            _dropdown.AddOptions(new List<string>()
            {
                "Spawn",
                "Gift",
                "Artefact",
                "Exit",
                "English",
                "BayShrine",
                "AddBlood",
            });
            _dropdown.value = 0;
        }

        public void RunTest()
        {
            var person = GameContainer.Players.Person;
            switch (_dropdown.value )
            {
                case 0: Spawn(); break;
                case 1: Gift(); break;
                case 2: person.Artefact.Next(UnityEngine.Random.Range(5, 10)); ; break;
                case 3: Vurbiri.EntryPoint.Transition.Exit(); break;
                case 4: Localization.Instance.SwitchLanguage(SystemLanguage.English); break;
                case 5: GameContainer.Humans[PlayerId.AI_01].BuyEdificeUpgrade(GameContainer.Crossroads[CROSS.NEAR[0]]); break;
                case 6: AddBlood(13); break;
                default: return;
            }
        }

        private void AddBlood(int value)
        {
            for (int i = 0; i < PlayerId.HumansCount; ++i)
                GameContainer.Humans[i].Resources.AddBlood(value);

            MessageBox.Open("test", MBButtonId.Ok, MBButtonId.Cancel);
        }

        private void Gift()
        {
            int giver = PlayerId.AI_01;
            string text = Localization.Instance.GetText(giftMsg);

            MainCurrencies gift = new();
            gift.RandomAddRange(5);

            StringBuilder sb = new(TAG.ALING_CENTER, 256);
            sb.Append(GameContainer.UI.PlayerNames[giver]); sb.Append(" "); sb.AppendLine(text);
            gift.PlusToStringBuilder(sb); sb.Append(TAG.ALING_OFF);

           
            GameContainer.Players.Person.OnGift(giver, gift, sb.ToString());
        }

        private void Spawn()
        {
            //_artefact.Next(UnityEngine.Random.Range(2, 10));

            var person = GameContainer.Players.Person;

            //person.SpawnTest(WarriorId.Militia, HEX.RightUp);
            person.SpawnTest(WarriorId.Solder, HEX.Right);
            //person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            //person.SpawnTest(WarriorId.Warlock, HEX.Left);
            //person.SpawnTest(WarriorId.Knight, HEX.LeftUp);

            person.SpawnTest(WarriorId.Militia, HEX.RightDown);
            //person.SpawnDemonTest(DemonId.Bomb, Key.Zero);

            person.SpawnDemonTest(DemonId.Imp, HEX.RightUp);
            //person.SpawnDemonTest(DemonId.Bomb, HEX.Right);
            //person.SpawnDemonTest(DemonId.Grunt, HEX.LeftDown);
            //person.SpawnDemonTest(DemonId.Fatty, HEX.Left);
            //person.SpawnDemonTest(DemonId.Boss, HEX.LeftUp);

            //person.SpawnTest(WarriorId.Knight, 2);
            //person.SpawnDemonTest(DemonId.Boss, 5);

            //person.SpawnDemonTest(DemonId.Imp, 5);

            //GameContainer.Players.GetAI(PlayerId.AI_01).SpawnTest(WarriorId.Militia, 2);
            //GameContainer.Players.GetAI(PlayerId.AI_02).SpawnTest(WarriorId.Wizard, 2);
        }

        public void ShowKey()
        {
            Dictionary<Key, Hexagon> hexagons = GameContainer.Hexagons;
            foreach (var hex in hexagons.Values)
                hex.Caption.ShowKey_Ed();
        }
        WaitAll _all;
        public void Testing()
        {

            StartCoroutine(TestCoroutine());
        }
        public void Testing2()
        {

            _all.Stop();
        }

        IEnumerator TestCoroutine()
        {
            _all = new(this);
            yield return _all.Add(TestCoroutine1(), TestCoroutine2(), TestCoroutine4());
            print($"RunAll {_all.Count}");
        }

        IEnumerator TestCoroutine1()
        {
            yield return new WaitForSeconds(4f);
            print("TestCoroutine1");
        }

        IEnumerator TestCoroutine2()
        {
            yield return TestCoroutine3();
            print("TestCoroutine2");
        }

        IEnumerator TestCoroutine3()
        {
            yield return new WaitRealtime(3f);
            print("TestCoroutine3");
        }

        IEnumerator TestCoroutine4()
        {
            yield return StartCoroutine(TestCoroutine5());
            print("TestCoroutine4");
        }

        IEnumerator TestCoroutine5()
        {
            yield return new WaitForSecondsRealtime(5f);
            print("TestCoroutine5");
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
                    x = MathI.Abs(i);
            }
            void TestValues(int value)
            {
                for (int i = -value; i < value; ++i)
                    if (Math.Abs(i) != MathI.Abs(i))
                        Log.Error($"ABS Error [{i}] -> Math{Math.Abs(i)} != MathI{MathI.Abs(i)}");
            }
        }

        public void MathIComparisonTesting()
        {
            print($"775 != 775: {MathI.NotEqual(775, 775) < 0}");
            print($"-75 != 891: {MathI.NotEqual(-75, 891) < 0}");
            print($"775 != -91: {MathI.NotEqual(775, -91) < 0}");
            print($"175 != 891: {MathI.NotEqual(175, 891) < 0}");
            print($"775 != 191: {MathI.NotEqual(775, 191) < 0}");
            print("===============================================");
            print($"775 == 775: {MathI.Equal(775, 775) < 0}");
            print($"-75 == 891: {MathI.Equal(-75, 891) < 0}");
            print($"775 == -91: {MathI.Equal(775, -91) < 0}");
            print($"175 == 891: {MathI.Equal(175, 891) < 0}");
            print($"775 == 191: {MathI.Equal(775, 191) < 0}");
            print("===============================================");
            print($"775 <= 775: {MathI.LessOrEqual(775, 775) < 0}");
            print($"-75 <= 891: {MathI.LessOrEqual(-75, 891) < 0}");
            print($"775 <= -91: {MathI.LessOrEqual(775, -91) < 0}");
            print($"175 <= 891: {MathI.LessOrEqual(175, 891) < 0}");
            print($"775 <= 191: {MathI.LessOrEqual(775, 191) < 0}");
            print("===============================================");
            print($"775 > 775: {MathI.Greater(775, 775) < 0}");
            print($"-75 > 891: {MathI.Greater(-75, 891) < 0}");
            print($"775 > -91: {MathI.Greater(775, -91) < 0}");
            print($"175 > 891: {MathI.Greater(175, 891) < 0}");
            print($"775 > 191: {MathI.Greater(775, 191) < 0}");
            print("===============================================");
            print($"775 >= 775: {MathI.GreaterOrEqual(775, 775) < 0}");
            print($"-75 >= 891: {MathI.GreaterOrEqual(-75, 891) < 0}");
            print($"775 >= -91: {MathI.GreaterOrEqual(775, -91) < 0}");
            print($"175 >= 891: {MathI.GreaterOrEqual(175, 891) < 0}");
            print($"775 >= 191: {MathI.GreaterOrEqual(775, 191) < 0}");
            print("===============================================");
            print($"775 < 775: {MathI.Less(775, 775) < 0}");
            print($"-75 < 891: {MathI.Less(-75, 891) < 0}");
            print($"775 < -91: {MathI.Less(775, -91) < 0}");
            print($"175 < 891: {MathI.Less(175, 891) < 0}");
            print($"775 < 191: {MathI.Less(775, 191) < 0}");
            print("===============================================");
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
