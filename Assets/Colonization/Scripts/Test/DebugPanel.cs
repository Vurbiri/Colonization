using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class DebugPanel : MonoBehaviour
	{
        private TMP_Dropdown _dropdown;

        private void Start()
        {
            _dropdown = GetComponentInChildren<TMP_Dropdown>();
            _dropdown.ClearOptions();
            _dropdown.AddOptions(new List<string>()
            {
                "Spawn",
                "Artefact",
                "AddBlood",
            });
            _dropdown.value = 0;

            //SwitchEnable();
            GameContainer.InputController.OnDebug.Add(SwitchEnable);
        }

        public void Invoke()
        {
            var person = GameContainer.Person;
            switch (_dropdown.value)
            {
                case 0: Spawn(); break;
                case 1: person.Artefact.Next(UnityEngine.Random.Range(5, 10)); ; break;
                case 2: AddBlood(13); break;
                default: return;
            }
        }

        private void AddBlood(int value)
        {
            for (int i = 0; i < PlayerId.HumansCount; ++i)
                GameContainer.Humans[i].Resources.AddBlood(value);

            MessageBox.Open("test", MBButtonId.Ok, MBButtonId.Cancel);
        }

        private void Spawn()
        {
            //_artefact.Next(UnityEngine.Random.Range(2, 10));

            var person = GameContainer.Person;

            person.SpawnTest(WarriorId.Militia, HEX.RightUp);
            person.SpawnTest(WarriorId.Solder, HEX.Right);
            //person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            //person.SpawnTest(WarriorId.Warlock, HEX.Left);
            //person.SpawnTest(WarriorId.Knight, HEX.LeftUp);

            person.SpawnTest(WarriorId.Militia, HEX.RightDown);
            //person.SpawnDemonTest(DemonId.Bomb, Key.Zero);

            //person.SpawnDemonTest(DemonId.Imp, HEX.RightUp);
            //person.SpawnDemonTest(DemonId.Bomb, HEX.Right);
            //person.SpawnDemonTest(DemonId.Grunt, HEX.LeftDown);
            //person.SpawnDemonTest(DemonId.Fatty, HEX.Left);
            //person.SpawnDemonTest(DemonId.Boss, HEX.LeftUp);

            //person.SpawnTest(WarriorId.Knight, 2);
            //person.SpawnDemonTest(DemonId.Boss, 5);

            //person.SpawnDemonTest(DemonId.Imp, 5);
        }

        private void SwitchEnable()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
