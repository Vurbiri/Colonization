using System;
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
        [Button("Testing"), Range(-10f, 10f)]
        public float value;

        public UsedSkills<WarriorId> skills;

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
            for (int i = 0; i < PlayerId.HumansCount; i++)
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

            person.SpawnTest(WarriorId.Militia, HEX.RightUp);
            person.SpawnTest(WarriorId.Solder, HEX.Right);
            //person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            //person.SpawnTest(WarriorId.Warlock, HEX.Left);
            person.SpawnTest(WarriorId.Knight, HEX.LeftUp);

            //person.SpawnTest(WarriorId.Knight, HEX.Left);
            //person.SpawnDemonTest(DemonId.Fatty, Key.Zero);

            //person.SpawnDemonTest(DemonId.Imp, HEX.RightUp);
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

        public void Testing()
        {
            StringTest(); StringTest();
        }

        public void StringTest()
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

            print("----------------- Add -------------------------");
            print($"List: {test1}");
            print($"Roster: {test2}");
            print("-----------------------------------------------");
            print($"List/Roster:  {test1 / test2}");
            print("===============================================");

            Thread.Sleep(100);
            stopWatch.Restart();
            TestListContains(list);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestHashContains(set);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            print("----------------- Contains --------------------");
            print($"List: {test1}");
            print($"Roster: {test2}");
            print("-----------------------------------------------");
            print($"List/Roster:  {test1 / test2}");
            print("===============================================");

            Thread.Sleep(100);
            stopWatch.Restart();
            TestListRemove(list);
            stopWatch.Stop();
            test2 = stopWatch.ElapsedTicks;

            stopWatch.Restart();
            TestHashRemove(set);
            stopWatch.Stop();
            test1 = stopWatch.ElapsedTicks;

            print("----------------- Remove ----------------------");
            print($"List: {test1}");
            print($"Roster: {test2}");
            print("-----------------------------------------------");
            print($"List/Roster:  {test1 / test2}");
            print("===============================================");

            void TestListAdd(List<int> list)
            {
                for(int i = 0; i < 100; i++)
                    list.Add(i);
            }
            void TestHashAdd(Roster<int> set)
            {
                for (int i = 0; i < 100; i++)
                    set.Add(i);
            }
            void TestListRemove(List<int> list)
            {
                for (int i = 200; i >= 0; i--)
                    list.Remove(i);
            }
            void TestHashRemove(Roster<int> set)
            {
                for (int i = 200; i >= 0; i--)
                    set.Remove(i);
            }
            void TestListContains(List<int> list)
            {
                for (int i = 200; i >= 0; i--)
                    list.Contains(i);
            }
            void TestHashContains(Roster<int> set)
            {
                for (int i = 200; i >= 0; i--)
                    set.Contains(i);
            }
        }

        public void MathITesting()
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
    }
}
