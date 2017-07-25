using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using System.Text;

namespace TalkedToyou
{
	public class ModEntry : Mod
	{
		private MouseState MState;

        private Point ClickPoint => new Point(this.MState.X, this.MState.Y);


		public override void Entry(IModHelper helper)
			{
				ControlEvents.KeyPressed += this.ControlEvents_KeyPress;
				TimeEvents.AfterDayStarted += this.TimeEvents_NewDay;
			}

        private static System.Collections.Generic.Stack<Dialogue> GetCurrentDialogue(NPC character)
        {
            return character.CurrentDialogue;
        }
		
		// TalkedToYou in global scope
		public String[] TalkedToYou = {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"}; 

		public Boolean newDay = false;
		private void TimeEvents_NewDay(object sender, EventArgs e){
			newDay = true;
		}

        private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
        {
            if (Context.IsWorldReady && Context.IsPlayerFree && e.KeyPressed.ToString() == "Q") // when player presses Q
            {
				var allCharacters = new String[28] {"Alex", "Elliott", "Shane", "Sebastian", "Harvey", "Abigail", "Emily", "Haley", "Leah", "Maru", "Penny", "Caroline", "Clint", "Demetrius", "Evelyn", "George", "Gus", "Jas", "Jodi", "Lewis", "Marnie", "Linus", "Pam", "Pierre", "Robin", "Vincent", "Willy", "Wizard"};

				string currLocation = Game1.currentLocation.ToString();
				String[] locArr = currLocation.Split('.');
				String location = locArr[1];

				if(location == "Locations"){
					location = locArr[2];
				}else if(location == "GameLocation"){
					location = "this location";
				}

				
				if (Context.IsWorldReady) {

				// should include all base game players
				var NotTalked = new String[28] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};
				
				// if the person is here, they'll be in this array. resets on every location change.
				var isHere = new String[28] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};

				while(newDay == true)
				{
					int x = 0;
					while(x <= 27){
						NotTalked[x] = "null";
						TalkedToYou[x] = "null";
						isHere[x] = "null";
						x++;
					}
					newDay = false;
				}

				foreach (NPC character in Game1.currentLocation.characters) // see who is in the area
					{
						if(character.ToString() == "StardewValley.NPC" && !(character.ToString() == "StardewValley.Characters.Child")) // Only works on NPCs -- no monsters or children!
						{
							int talkStack = GetCurrentDialogue(character).Count; // get the dialogue stack for each character
							int j = 0;
							foreach (String name in isHere){
								if(isHere[j] == character.name){
									break;
								}
								if(isHere[j] == "null"){
									isHere[j] = character.name;
									break;
								}
								
								j++;
							}	
							// if they have no more dialogue...
							if(talkStack <= 0){
								int k = 0;
								foreach(String name in TalkedToYou) {
									// if the person with no dialogue is in the not talked array, take them out
									if(NotTalked[k] == character.name){
										NotTalked[k] = "null";
									}
									// and put them in the talked to array
									if(TalkedToYou[k] == "null"){
											TalkedToYou[k] = character.name;					
											break;
										}
										k++;
									}
								
													
							}
						}

					}

					foreach(String name in allCharacters){
						
						int y = 0;
						bool talkedTo = false;
						while(y<=26){
							if(TalkedToYou[y] == name){
								talkedTo = true;
								break;
							}
							y++;
						}

						if(talkedTo == false){
							int z = 0;
							while(z <= 27){
								string check2 = NotTalked[z];
								if(NotTalked[z] == "null"){
									NotTalked[z] = name;
									break;
								}
								z++;
							}
						}else if(talkedTo == true){
							int s = 0;
							while(s <= 27){
								if(NotTalked[s] == name){
									NotTalked[s] = "null";
								}
								s++;
							}
						}
					}

					// NotTalked, isHere, & TalkedToYou	
					String notTalkedToHere = "";
					String notTalkedNotHere = "";
					bool show = false;
					int num1 = 1;
					int num2 = 1;

					foreach(String name in isHere){
						int f = 0;
						while(f <= 27){
							if(name == NotTalked[f] && name != "null"){
								notTalkedToHere += $"{num1}. {name} ";
								num1++;
								show = true;
							}
							f++;
						}
					}			
						int g = 0;
						while(g <= 27){
							if(NotTalked[g] != "null"){
								notTalkedNotHere += $"{num2}. {NotTalked[g]} ";
								num2++;
							}
							g++;
						}
				
					// finally, draw the messages!
					if(show == true){
						Game1.drawLetterMessage($"Here are the players in {location} who you haven't talked to: {notTalkedToHere}. You still need to talk to {notTalkedNotHere}");
					}else{	
						Game1.drawLetterMessage($"There is no one to talk to in {location}. You still need to talk to {notTalkedNotHere}");
					}
				}
            }
       	 }
	}
}
