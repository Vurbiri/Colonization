using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
	public class Test : MonoBehaviour
	{
        [Space]
        public FileIdAndKey giftMsg;

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
            });
            _dropdown.value = 0;
        }

        public void RunTest()
        {
            switch(_dropdown.value )
            {
                case 0: Spawn(); break;
                case 1: Gift(); break;
                case 2: GameContainer.Players.Person.Artefact.Next(UnityEngine.Random.Range(90, 100)); ; break;
                case 3: Vurbiri.EntryPoint.Transition.Exit(); break;
                case 4: Localization.Instance.SwitchLanguage(SystemLanguage.English); break;
                case 5: GameContainer.Players.Humans[PlayerId.AI_01].BuyEdificeUpgrade(GameContainer.Crossroads[CROSS.NEAR.Rand()]); break;
                default: return;
            }
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
            Debug.Log("Удалить Тесты в ArtefactPanel");

            //_artefact.Next(UnityEngine.Random.Range(2, 10));

            var person = GameContainer.Players.Person;

            person.SpawnTest(WarriorId.Militia, HEX.RightUp);
            person.SpawnTest(WarriorId.Solder, HEX.Right);
            //person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            //person.SpawnTest(WarriorId.Warlock, HEX.Left);
            person.SpawnTest(WarriorId.Knight, HEX.LeftUp);

            //person.SpawnTest(WarriorId.Knight, HEX.Left);
            person.SpawnDemonTest(DemonId.Fatty, Key.Zero);

            //person.SpawnDemonTest(DemonId.Imp, HEX.RightUp);
            //person.SpawnDemonTest(DemonId.Bomb, HEX.Right);
            //person.SpawnDemonTest(DemonId.Grunt, HEX.LeftDown);
            person.SpawnDemonTest(DemonId.Fatty, HEX.Left);
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
    }
}
