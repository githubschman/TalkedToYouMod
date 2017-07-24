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
				ControlEvents.MouseChanged += this.ControlEvents_MouseChanged;
			}
			private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
			{
			
			}

        private static System.Collections.Generic.Stack<Dialogue> GetCurrentDialogue(NPC character)
        {
            return character.CurrentDialogue;
        }
		
		// includes characters you talked to
		// MOVED THIS TO GLOBAL SCOPE? made it an empty string array.
		public String[] TalkedToYou = {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"}; 

        private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
        {

            if (Context.IsWorldReady && Context.IsPlayerFree && e.KeyPressed.ToString() == "Q")
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

				// should include ALL characters and players children
				var NotTalked = new String[28] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};
				
				// if the person is here, they'll be in this array. resets on every location change.
				var isHere = new String[28] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};

				if(Game1.newDay){
					int x = 0;
					while(x >= 27){
						NotTalked[x] = null;
						TalkedToYou[x] = null;
						isHere[x] = null;
						x++;
					}
				}


				foreach (NPC character in Game1.currentLocation.characters)
					{

						if(character.ToString() == "StardewValley.NPC" && !(character.ToString() == "StardewValley.Characters.Child"))
						{
							int talkStack = GetCurrentDialogue(character).Count;

							// add name to isHere array
							int j = 0;
							foreach (String name in isHere){
								if(isHere[j] == character.name){
									break;
								}

								if(isHere[j] == "null"){
									isHere[j] = character.name;
									String check = isHere[j];
									break;
								}
								
								j++;
							}

							
							// if they have no more dialogue...
							if(talkStack <= 0){
								int k = 0;
								foreach(String name in TalkedToYou) {


									this.Monitor.Log($"Going through TalkedToYou!, checking on Not Talked: {NotTalked[k]}");

									// if the person with no dialogue is in the not talked array, take them out
									if(NotTalked[k] == character.name){
										this.Monitor.Log($"you talked to {character.name} so we are removing them from the Not Talked array!");
										NotTalked[k] = "null";
									}
									// and put them in the talked to array
									if(TalkedToYou[k] == "null"){
											TalkedToYou[k] = character.name;
											
											String check2 = TalkedToYou[k];
											this.Monitor.Log($"You talked to {character.name} today! check: {check2}");
											// THIS WORKS!
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

							string check = TalkedToYou[y];
							this.Monitor.Log($"looking to see if {name} is in talked to you, at {check}");

							// if they are in the arrays talkedtoyou, don't include in nottalked
							if(TalkedToYou[y] == name){
								this.Monitor.Log($"Yep, you talked to {name}!");
								talkedTo = true;
								// THIS WORKS!
								break;
							}
							y++;
						}

						if(talkedTo == false){
							int z = 0;
							while(z <= 27){
								string check2 = NotTalked[z];
								this.Monitor.Log($"NOT talked to array: {check2}");
								if(NotTalked[z] == "null"){
									this.Monitor.Log($"havent talked to {name} yet today, adding them to the not talked to array!");
									NotTalked[z] = name;
									break;
								}
								z++;
							}
						}else if(talkedTo == true){
							// Talked to whomever, so remove them from notTalked
							int s = 0;
							while(s <= 27){
								if(NotTalked[s] == name){
									this.Monitor.Log($"Talked to {name} so removing them from Not Talked, lol this is crazy");
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
						int num = 1;

						foreach(String name in isHere){
							int f = 0;
							while(f <= 27){
								if(name == NotTalked[f] && name != "null"){
									notTalkedToHere += $"{num}. {name} ";
									num++;
									show = true;
								}
								f++;
							}
							
						}
						


						foreach(String name in NotTalked){
							int f = 0;
							while(f <= 27){
								int num = 1;
								if(name != isHere[f] && name != "null"){
									notTalkedNotHere += $"{num}. {name} ";
								
									num++;
								}
								f++;
							}
						}
						
						if(show == true){
							Game1.drawObjectDialogue($"Here are the players in {location} who you haven't talked to: {notTalkedToHere}");
						}else{
							Game1.drawObjectDialogue($"There is no one to talk to in {location}.");
						}
					
						// Game1.drawObjectDialogue($"You still need to talk to: {notTalkedNotHere}");
				}
            }
       	 }
	}
}
