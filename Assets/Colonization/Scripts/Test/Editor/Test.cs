using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
	public class Test : MonoBehaviour
	{
        public void CreateUnits()
        {
            Debug.Log("Удалить Тесты в ArtefactPanel");

            //_artefact.Next(UnityEngine.Random.Range(2, 10));

            var person = GameContainer.Players.Person;

            person.SpawnTest(WarriorId.Militia, HEX.RightUp);
            person.SpawnTest(WarriorId.Solder, HEX.Right);
            //person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            //person.SpawnTest(WarriorId.Warlock, HEX.Left);
            //person.SpawnTest(WarriorId.Knight, HEX.LeftUp);

            person.SpawnDemonTest(DemonId.Fatty, Key.Zero);

            //person.SpawnDemonTest(DemonId.Imp, HEX.RightUp);
            //person.SpawnDemonTest(DemonId.Bomb, HEX.Right);
            //person.SpawnDemonTest(DemonId.Grunt, HEX.LeftDown);
            //person.SpawnDemonTest(DemonId.Fatty, HEX.Left);
            //person.SpawnDemonTest(DemonId.Boss, HEX.LeftUp);

            //person.SpawnTest(WarriorId.Knight, 2);
            person.SpawnDemonTest(DemonId.Boss, 2);

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
