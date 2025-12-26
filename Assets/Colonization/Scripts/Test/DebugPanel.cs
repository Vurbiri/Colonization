using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class DebugPanel : MonoBehaviour
	{
		private TMP_Dropdown _dropdown;
		private TMP_InputField _input;
		private int _inputValue;

		private void Start()
		{
			_dropdown = GetComponentInChildren<TMP_Dropdown>();
			_dropdown.ClearOptions();
			_dropdown.AddOptions(new List<string>()
			{
				"Res.+",
				"Blood+",
				"Warrior",
				"Demon",
				"Chaos-",
				"Chaos+",
				"Score+",
				"Artefact+",
				"Spawn",
			});
			_dropdown.value = 0;

			_input = GetComponentInChildren<TMP_InputField>();
			_input.onEndEdit.AddListener(OnInput);

			//gameObject.SetActive(false);
			GameContainer.InputController.OnDebug.Add(SwitchEnable);
		}

		public void Invoke()
		{
			switch (_dropdown.value)
			{
				case 0: GameContainer.Person.Resources.Add(_inputValue); break;
				case 1: GameContainer.Person.Resources.Blood.Add(_inputValue); break;
				case 2: WarriorSpawn(_inputValue); break;
				case 3: DemonSpawn(_inputValue); break;
				case 4: GameContainer.Chaos.Add(-11 * _inputValue); break;
				case 5: GameContainer.Chaos.Add(+11 * _inputValue); break;
				case 6: GameContainer.Score.Add(PlayerId.Person, 5 * _inputValue); break;
				case 7: Artefact(); break;
				case 8: Spawn(); break;

				default: break;
			}
		}

		public void OnInput(string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				_input.text = "0";
				_inputValue = 0;
				return;
			}

			try
			{
				_inputValue = Int32.Parse(input);
			}
			catch
			{
				_input.text = "0";
				_inputValue = 0;
			}
		}

		private void Artefact()
		{
			var perks = GameContainer.Person.Perks;
			if (!perks.IsPerkLearned(MilitaryPerksId.Type, MilitaryPerksId.IsArtefact_1))
				perks.Learn(MilitaryPerksId.Type, MilitaryPerksId.IsArtefact_1);
			else
				GameContainer.Person.Artefact.Next(_inputValue);
		}

		private void SwitchEnable()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}

		#region ================== Spawn ============================
		public void WarriorSpawn(int warriorId)
		{
			if (warriorId > WarriorId.Knight || GameContainer.Person.IsMaxWarriors) return;

			Hexagon hexagon;
			while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanWarriorEnter) ;
			GameContainer.Person.Recruiting(warriorId, hexagon);
		}
		public void WarriorSpawn(int warriorId, Key key)
		{
			if (warriorId > WarriorId.Knight || GameContainer.Person.IsMaxWarriors) return;

			Hexagon hexagon;
			if ((hexagon = GameContainer.Hexagons[key]).CanWarriorEnter)
				GameContainer.Person.Recruiting(warriorId, hexagon);
		}
		
		public void DemonSpawn(int demonId)
		{
			if (demonId > DemonId.Boss) return;

			Hexagon hexagon = GameContainer.Hexagons[Key.Zero];
			while (!hexagon.CanDemonEnter) hexagon = GameContainer.Hexagons[HEX.NEARS.Random];
			GameContainer.Satan.Spawn(demonId, hexagon);
		}
		public void DemonSpawn(int demonId, Key key)
		{
			Hexagon hexagon;
			if (demonId <= DemonId.Boss && (hexagon = GameContainer.Hexagons[key]).CanDemonEnter)
				GameContainer.Satan.Spawn(demonId, hexagon);
		}

		private void Spawn()
		{
			WarriorSpawn(WarriorId.Militia, HEX.RightUp);
			//WarriorSpawn(WarriorId.Solder, HEX.Right);
			//WarriorSpawn(WarriorId.Wizard, HEX.LeftDown);
			//WarriorSpawn(WarriorId.Warlock, HEX.Left);
			//WarriorSpawn(WarriorId.Knight, HEX.LeftUp);

			//WarriorSpawn(WarriorId.Militia, HEX.RightDown);
			DemonSpawn(DemonId.Imp, HEX.Right);

			//DemonSpawn(DemonId.Imp, HEX.RightUp);
			//DemonSpawn(DemonId.Bomb, HEX.Right);
			//DemonSpawn(DemonId.Grunt, HEX.LeftDown);
			//DemonSpawn(DemonId.Fatty, HEX.Left);
			//DemonSpawn(DemonId.Boss, HEX.LeftUp);

			//WarriorSpawn(WarriorId.Knight);
			//DemonSpawn(DemonId.Boss);
		}
		#endregion
	}
}
