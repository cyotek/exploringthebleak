/***********************************************************************************\
 * Copyright (c) 2012, Nathaniel Inman                                             *
 * All rights reserved.                                                            *
 *                                                                                 *
 * Redistribution and use in source and binary forms, with or without              *
 * modification, are permitted provided that the following conditions are met:     *
 *     * Redistributions of source code must retain the above copyright            *
 *       notice, this list of conditions and the following disclaimer.             *
 *     * Redistributions in binary form must reproduce the above copyright         *
 *       notice, this list of conditions and the following disclaimer in the       *
 *       documentation and/or other materials provided with the distribution.      *
 *     * Neither the name of the The Other Experiment Studio nor the               *                
 *       names of its contributors may be used to endorse or promote products      *
 *       derived from this software without specific prior written permission.     *
 *                                                                                 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND *
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED   *
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE          *
 * DISCLAIMED. IN NO EVENT SHALL NATHANIEL INMAN BE LIABLE FOR ANY                 *
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES      *
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;    *
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND     *
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT      *
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS   *
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                    *
\***********************************************************************************/
function resetcheckKey(e){
	this.onkeydown=checkKey;
} //end function 
function moveLocation(x,y){
	if (map[x][y]>=1 && mobMap[x][y]==0){ //location is empty, move is acceptable
		playerFOV(player.fieldOfViewRange,player.mapPos.x,player.mapPos.y,0);
		player.mapPos.x=x;player.mapPos.y=y; //movement was successful
		playerFOV(player.fieldOfViewRange,player.mapPos.x,player.mapPos.y,1);
		tick();
	} else if (map[x][y]>=1 && mobMap[x][y]>=1){ //mob is at the location, attempt to attack
		hitMob(x,y);
		tick();
	} else { //location is a wall, can't move onto walls
		textOutput("Can't move there.");
		tick();
	} //end if
} //end function				
function checkKey(e){
	e=e || window.event;
	if (curState=='Play'){
		if (e.keyCode=='37' && player.mapPos.x>0 && panelOpen==0 || /* left arrow key (west) */
			e.keyCode=='100' && player.mapPos.x>0 && panelOpen==0){ /* numpad 4       (west) */
			moveLocation(player.mapPos.x-1,player.mapPos.y);
		} else if (e.keyCode=='38' && player.mapPos.y>0 && panelOpen==0 || /* up arrow key (north) */
				   e.keyCode=='104' &&player.mapPos.y>0 && panelOpen==0){  /* numpad 8     (north) */
			moveLocation(player.mapPos.x,player.mapPos.y-1);
		} else if (e.keyCode=='39' && player.mapPos.x<size && panelOpen==0 || /* right arrow key (east) */
				   e.keyCode=='102' &&player.mapPos.x<size && panelOpen==0){  /* numpad 6        (east) */
			moveLocation(player.mapPos.x+1,player.mapPos.y);
		} else if (e.keyCode=='40' && player.mapPos.y<size && panelOpen==0 || /* down arrow key (south) */
				   e.keyCode=='98' && player.mapPos.y<size && panelOpen==0){  /* numpad 2       (south) */
			moveLocation(player.mapPos.x,player.mapPos.y+1);
		} else if (e.keyCode=='97' && player.mapPos.x>0 && player.mapPos.y<size && panelOpen==0){ //1 numpad (southwest)
			moveLocation(player.mapPos.x-1,player.mapPos.y+1);
		} else if (e.keyCode=='99' && player.mapPos.x<size && player.mapPos.y<size && panelOpen==0){ //3 numpad (southeast)
			moveLocation(player.mapPos.x+1,player.mapPos.y+1);
		} else if (e.keyCode=='103' &&player.mapPos.x>0 && player.mapPos.y>0 && panelOpen==0){ //7 numpad (northwest)
			moveLocation(player.mapPos.x-1,player.mapPos.y-1);
		} else if (e.keyCode=='105' && player.mapPos.x<size && player.mapPos.y>0 && panelOpen==0){ //9 numpad (northeast)
			moveLocation(player.mapPos.x+1,player.mapPos.y-1);
		} else if (e.keyCode=='101' && panelOpen==0){ //5 numpad (--wait--)
			if (player.chp<player.mhp)player.chp++;
			if (player.cwp<player.mwp)player.cwp+=5;
			playerFOV(player.fieldOfViewRange,player.mapPos.x,player.mapPos.y,1);moveMobiles();player.cMove++;
			drawHP();drawWP();
		} else if (e.keyCode=='73'){ //inventory 'i'
			if (inventoryOpen==0 && panelOpen==0){
				drawPanel(200,500,'Inventory');
				if(playerInventory.cur==0){
					drawPanelTextReg("You aren't carrying anything.");
				}else{
					for(var i=1;i<=playerInventory.cur;i++){
						if(playerInventory.itemType[i]){
							drawPanelTextReg(i-1+". "+playerInventory.itemName[i]+"");
						}else{
							drawPanelTextReg(i-1+". "+playerInventory.itemName[i]+"");
						}//end if
					}//end if
				} //end if
				inventoryOpen=1;panelOpen=1;
			} else if (inventoryOpen==1 && panelOpen==1){
				clearPanel();
				inventoryOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode=='48' && inventoryOpen==1){lookInventory(0);
		} else if (e.keyCode=='49' && inventoryOpen==1){lookInventory(1);
		} else if (e.keyCode=='50' && inventoryOpen==1){lookInventory(2);
		} else if (e.keyCode=='51' && inventoryOpen==1){lookInventory(3);
		} else if (e.keyCode=='52' && inventoryOpen==1){lookInventory(4);
		} else if (e.keyCode=='53' && inventoryOpen==1){lookInventory(5);
		} else if (e.keyCode=='54' && inventoryOpen==1){lookInventory(6);
		} else if (e.keyCode=='55' && inventoryOpen==1){lookInventory(7);
		} else if (e.keyCode=='56' && inventoryOpen==1){lookInventory(8);
		} else if (e.keyCode=='57' && inventoryOpen==1){lookInventory(9);
		} else if (e.keyCode=='67'){ //drop 'c'
			if (characterOpen==0 && panelOpen==0){
				drawCharacterPanel();
				characterOpen=1;panelOpen=1;
			} else if (characterOpen==1 && panelOpen==1){
				clearPanel();
				characterOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode=='68'){ //drop 'd'
			if (dropOpen==0 && panelOpen==0){
				drawDropPanel();
				dropOpen=1;panelOpen=1;
			} else if (dropOpen==1 && panelOpen==1){
				clearPanel();
				dropOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode=='48' && dropOpen==1){popInventory(0);
		} else if (e.keyCode=='49' && dropOpen==1){popInventory(1);
		} else if (e.keyCode=='50' && dropOpen==1){popInventory(2);
		} else if (e.keyCode=='51' && dropOpen==1){popInventory(3);
		} else if (e.keyCode=='52' && dropOpen==1){popInventory(4);
		} else if (e.keyCode=='53' && dropOpen==1){popInventory(5);
		} else if (e.keyCode=='54' && dropOpen==1){popInventory(6);
		} else if (e.keyCode=='55' && dropOpen==1){popInventory(7);
		} else if (e.keyCode=='56' && dropOpen==1){popInventory(8);
		} else if (e.keyCode=='57' && dropOpen==1){popInventory(9);
		} else if (e.keyCode=='69' && e.shiftKey==false){ //equip 'e'
			if (equipOpen==0 && panelOpen==0){
				drawPanel(200,500,'Equip Item');
				equipOpen=1;panelOpen=1;
			} else if (equipOpen==1 && panelOpen==1){
				clearPanel();
				equipOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode=='76'){ //look 'l'
			if (lookOpen==0 && panelOpen==0){
				cx=player.mapPos.x;cy=player.mapPos.y;drawBlock();
				lookOpen=1;panelOpen=1;
			} else if (lookOpen==1 && panelOpen==1){
				clearPanel();
				lookOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode=='83'){ //shoot
			if (shootOpen==0 && panelOpen==0){
				cx=player.mapPos.x;cy=player.mapPos.y;drawBlock();
				shootOpen=1;panelOpen=1;
			} else if (shootOpen==1 && panelOpen==1){
				clearPanel();
				shootOpen=0;panelOpen=0;
			} //end if
		} else if (e.keyCode== '37' && lookOpen==1 && cx>0 || e.keyCode=='100' && lookOpen==1 && cx>0 ||
				   e.keyCode== '37' && shootOpen==1 && cx>0 || e.keyCode=='100' && shootOpen==1 && cx>0){ //west
			cx--;clearPanel();drawBlock();
		} else if (e.keyCode== '38' && lookOpen==1 && cy>0 || e.keyCode=='104' && lookOpen==1 && cy>0 ||
				   e.keyCode== '38' && shootOpen==1 && cy>0 || e.keyCode=='104' && shootOpen==1 && cy>0){ //north
			cy--;clearPanel();drawBlock();
		} else if (e.keyCode== '39' && lookOpen==1 && cx<size || e.keyCode=='102' && lookOpen==1 && cx<size ||
				   e.keyCode== '39' && shootOpen==1 && cx<size || e.keyCode=='102' && shootOpen==1 && cx<size){ //east
			cx++;clearPanel();drawBlock();
		} else if (e.keyCode=='40' && lookOpen==1 && cy<size || e.keyCode=='98' && lookOpen==1 && cy<size ||
				   e.keyCode=='40' && shootOpen==1 && cy<size || e.keyCode=='98' && shootOpen==1 && cy<size){ //south
			cy++;clearPanel();drawBlock();
		} else if (e.keyCode=='97' && lookOpen==1 && cx>0 && cy<size || e.keyCode=='97' && shootOpen==1 && cx>0 && cy<size){ //southwest
			cx--;cy++;clearPanel();drawBlock();
		} else if (e.keyCode=='99' && lookOpen==1 && cx<size && cy<size || e.keyCode=='99' && shootOpen==1 && cx<size && cy<size){ //southeast
			cx++;cy++;clearPanel();drawBlock();
		} else if (e.keyCode=='103' && lookOpen==1 && cx>0 && cy>0 || e.keyCode=='103' && shootOpen==1 && cx>0 && cy>0){ //northwest
			cx--;cy--;clearPanel();drawBlock();
		} else if (e.keyCode=='105' && lookOpen==1 && cx<size && cy>0 || e.keyCode=='105' && shootOpen==1 && cx<size && cy>0){ //northeast
			cx++;cy--;clearPanel();drawBlock();
		} else if (e.keyCode=='13' && lookOpen==1 || e.keyCode=='32' && lookOpen==1 || e.keyCode=='101' && lookOpen==1){
			if (map[cx][cy]==2 && mobMap[cx][cy]>0){
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>look</font> at a <font color='FF5555'>"+mobs[mobMap[cx][cy]].nam+"</font>. <br /><font color='559955'>"+mobs[mobMap[cx][cy]].des+"</font>");
			} else if (map[cx][cy]==2 && itemMap[cx][cy]>0 && items.curItem[itemMap[cx][cy]]==1){ //weapon
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>look</font> at a <font color='FFFF55'>"+items.weapons[itemMap[cx][cy]].name+"</font>. <br /><font color='559955'>"+items.weapons[itemMap[cx][cy]].description+"</font>");
			} else if (map[cx][cy]==2 && itemMap[cx][cy]>0){ //armor
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>look</font> at a <font color='FF55FF'>"+items.armor[itemMap[cx][cy]].name+"</font>. <br /><font color='559955'>"+items.armor[itemMap[cx][cy]].description+"</font>");
			} else if (cx==stairsUpX && cy==stairsUpY){
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>notice</font> stairs going <font color='55FF55'>up</font>.");
			} else if (cx==stairsDownX && cy==stairsDownY){
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>notice</font> stairs going <font color='55FF55'>down</font>.");
			} else if (map[cx][cy]==2 && deadMap[cx][cy]==1){
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You <font color='55FFFF'>notice</font> a pool of blood.");
			} else if (map[cx][cy]==2){
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You don't <font color='55FFFF'>see</font> anything interesting there.");
			} else if (map[cx][cy]==0 && visMap[cx][cy]==1){
				lookOpen=0;panelOpen=0;clearPanel();
				if (wallMap[cx][cy]==0){
					textOutput("Your <font color='55FFFF'>notice</font> some flora obscuring your path.");
				} else if (wallMap[cx][cy]==1){
					textOutput("You <font color='55FFFF'>notice</font> an unpassable patch of water obscuring your path.");
				} else if (wallMap[cx][cy]==2){
					textOutput("You <font color='55FFFF'>notice</font> the ground rises to obscure your path.");
				} else if (wallMap[cx][cy]==3){
					textOutput("You <font color='55FFFF'>notice</font> a rock obscuring your path.");
				} else if (wallMap[cx][cy]==4){
					textOutput("You <font color='55FFFF'>notice</font> a shrub obscuring your path.");
				} else if (wallMap[cx][cy]==5){
					textOutput("You <font color='55FFFF'>notice</font> a tree obscuring your path.");
				} else if (wallMap[cx][cy]==6){
					textOutput("You <font color='55FFFF'>notice</font> some lava obscuring your path.");
				} else if (wallMap[cx][cy]==7){
					textOutput("You <font color='55FFFF'>notice</font> a wall obscuring your path.");
				} //end if
			} else {
				lookOpen=0;panelOpen=0;clearPanel();
				textOutput("You can't <font color='55FFFF'>see</font> there, it's <font color='FF5555'>obscured</font>.");
			} //end if
		} else if (e.keyCode=='13' && shootOpen==1 || e.keyCode=='32' && shootOpen==1 || e.keyCode=='101' && shootOpen==1){
			if (map[cx][cy]==2 && mobMap[cx][cy]>0){
				shootOpen=0;panelOpen=0;var tmp;
				drawLine(cx,cy);
				setInterval(function lazer(){
					if (tmp==undefined){
						clearPanel();
						textOutput("You shoot and kill a <font color='FF5555'>"+mobs[mobMap[cx][cy]].nam+"</font>.<br /><font color='559955'>You gained "+mobDB.hit[mobs[mobMap[cx][cy]].type]+" experience.</font>");
						hitMob(cx,cy);
						tick();
						tmp=0;
					} else {
						clearInterval(lazer());
					}
				},200); //end setInterval
			} //end if
		} else if (e.keyCode=='69' && e.shiftKey==true){ //show current environment 'e'
			var vowel="a"
			if (environmentName.charAt(0)=="A")vowel="an";if (environmentName.charAt(0)=="E")vowel="an";
			if (environmentName.charAt(0)=="I")vowel="an";if (environmentName.charAt(0)=="O")vowel="an";
			if (environmentName.charAt(0)=="U")vowel="an";
			textOutput("You're standing in "+vowel+" <font color='"+environment.color+"'>"+environment.name+"</font> on depth "+player.cDepth+" of the Bleak.");
		} else if (e.keyCode=='188' && e.shiftKey==false){ //pickup ','
			pickupItem();
			tick();
		} else if (e.keyCode=='188' && e.shiftKey==true && player.mapPos.x==stairsUpX && player.mapPos.y==stairsUpY){ //go up
			player.cDepth--;
			generateMap();
			drawMap();
		} else if (e.keyCode=='190' && e.shiftKey==true && player.mapPos.x==stairsDownX && player.mapPos.y==stairsDownY){ //go down
			player.cDepth++;
			generateMap();
			drawMap();
		}//end if (is curState='Play'?)
	} //end if
} //end function
function pickupItem(){
	if(playerInventory.cur<playerInventory.max){
		playerInventory.cur++;
		if (items.curItem[itemMap[player.mapPos.x][player.mapPos.y]]==1){ //weapon
			playerInventory.itemName[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].name;
			playerInventory.itemType[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].type;
			playerInventory.itemNum[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].num;
			playerInventory.itemCharacter[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].character;
			playerInventory.itemWeight[playerInventory.cur]=0;
			playerInventory.itemEquipmentType[playerInventory.cur]=0;
			playerInventory.itemDamageType[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].damagetype;
			playerInventory.itemDescription[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].description;
			playerInventory.itemDamage[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].damage;
			playerInventory.itemTwoHanded[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].twoHanded;
			playerInventory.itemWorth[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].worth;
			playerInventory.itemBashAC[playerInventory.cur]=0;
			playerInventory.itemSlashAC[playerInventory.cur]=0;
			playerInventory.itemPierceAC[playerInventory.cur]=0;
			playerInventory.itemFistAC[playerInventory.cur]=0;
			playerInventory.itemExoticAC[playerInventory.cur]=0;
			playerInventory.itemLevelReq[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].levelReq;
			playerInventory.itemGenderReq[playerInventory.cur]=0;
			playerInventory.itemClassReq[playerInventory.cur]=items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].classReq;
			playerInventory.itemProficiencyReq[playerInventory.cur]=0;
			textOutput("You pickup a <font color='FFFF55'>"+items.weapons[itemMap[player.mapPos.x][player.mapPos.y]].name+"</font>.");
		} else { //armor
			playerInventory.itemName[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].name;
			playerInventory.itemType[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].type;
			playerInventory.itemNum[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].num;
			playerInventory.itemCharacter[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].character;
			playerInventory.itemWeight[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].weight;
			playerInventory.itemEquipmentType[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].equipmentType;
			playerInventory.itemDamageType[playerInventory.cur]=0;
			playerInventory.itemDescription[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].description;
			playerInventory.itemDamage[playerInventory.cur]=0;
			playerInventory.itemTwoHanded[playerInventory.cur]=0;
			playerInventory.itemWorth[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].worth;
			playerInventory.itemBashAC[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].bashAC;
			playerInventory.itemSlashAC[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].slashAC;
			playerInventory.itemPierceAC[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].pierceAC;
			playerInventory.itemFistAC[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].fistAC;
			playerInventory.itemExoticAC[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].exoticAC;
			playerInventory.itemLevelReq[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].levelReq;
			playerInventory.itemGenderReq[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].genderReq;
			playerInventory.itemClassReq[playerInventory.cur]=0;
			playerInventory.itemProficiencyReq[playerInventory.cur]=items.armor[itemMap[player.mapPos.x][player.mapPos.y]].proficiencyReq;
			textOutput("You pickup a <font color='FF55FF'>"+items.armor[itemMap[player.mapPos.x][player.mapPos.y]].name+"</font>.");
		} //end if
		itemMap[player.mapPos.x][player.mapPos.y]=0;
		drawLocation(player.mapPos.x,player.mapPos.y);
	}else{
		textOutput("<font color='FF5555'>Your inventory is full.</font>")
	} //end if
} //end function
function lookInventory(num){
	if(num<playerInventory.cur){
		if (playerInventory.itemType[num+1]==1){ //weapon
			textOutput("You <font color='55FFFF'>look</font> at a <font color='FFFF55'>"+playerInventory.itemName[num+1]+"</font>. <br /><font color='559955'>"+playerInventory.itemDescription[num+1]+"</font>");
		} else { //armor
			textOutput("You <font color='55FFFF'>look</font> at a <font color='FF55FF'>"+playerInventory.itemName[num+1]+"</font>. <br /><font color='559955'>"+playerInventory.itemDescription[num+1]+"</font>");
		} //end if
	}else{
		textOutput("<font color='FF5555'>Nothing there to look at.</font>");
	} //end if
}//end function
function popInventory(num){
	if(num<playerInventory.cur){
		playerInventory.cur--;
		if(playerInventory.itemType[num+1]==0){ //armor
			textOutput("You drop a <font color='FF55FF'>"+playerInventory.itemName[num+1]+"</font>.");
		}else{ //weapon
			textOutput("You drop a <font color='FFFF55'>"+playerInventory.itemName[num+1]+"</font>.");
		} //end if
		for(var i=num+1;i<=10;i++){
			if(i==10){
				playerInventory.itemName[          i]=0;
				playerInventory.itemType[          i]=0;
				playerInventory.itemNum[           i]=0;
				playerInventory.itemCharacter[     i]=0;
				playerInventory.itemWeight[        i]=0;
				playerInventory.itemEquipmentType[ i]=0;
				playerInventory.itemDamageType[    i]=0;
				playerInventory.itemDescription[   i]=0;
				playerInventory.itemDamage[        i]=0;
				playerInventory.itemTwoHanded[     i]=0;
				playerInventory.itemWorth[         i]=0;
				playerInventory.itemBashAC[        i]=0;
				playerInventory.itemSlashAC[       i]=0;
				playerInventory.itemPierceAC[      i]=0;
				playerInventory.itemFistAC[        i]=0;
				playerInventory.itemExoticAC[      i]=0;
				playerInventory.itemLevelReq[      i]=0;
				playerInventory.itemGenderReq[     i]=0;
				playerInventory.itemClassReq[      i]=0;
				playerInventory.itemProficiencyReq[i]=0;
			}else{
				playerInventory.itemName[          i]=playerInventory.itemName[          i+1];
				playerInventory.itemType[          i]=playerInventory.itemType[          i+1];
				playerInventory.itemNum[           i]=playerInventory.itemNum[           i+1];
				playerInventory.itemCharacter[     i]=playerInventory.itemCharacter[     i+1];
				playerInventory.itemWeight[        i]=playerInventory.itemWeight[        i+1];
				playerInventory.itemEquipmentType[ i]=playerInventory.itemEquipmentType[ i+1];
				playerInventory.itemDamageType[    i]=playerInventory.itemDamageType[    i+1];
				playerInventory.itemDescription[   i]=playerInventory.itemDescription[   i+1];
				playerInventory.itemDamage[        i]=playerInventory.itemDamage[        i+1];
				playerInventory.itemTwoHanded[     i]=playerInventory.itemTwoHanded[     i+1];
				playerInventory.itemWorth[         i]=playerInventory.itemWorth[         i+1];
				playerInventory.itemBashAC[        i]=playerInventory.itemBashAC[        i+1];
				playerInventory.itemSlashAC[       i]=playerInventory.itemSlashAC[       i+1];
				playerInventory.itemPierceAC[      i]=playerInventory.itemPierceAC[      i+1];
				playerInventory.itemFistAC[        i]=playerInventory.itemFistAC[        i+1];
				playerInventory.itemExoticAC[      i]=playerInventory.itemExoticAC[      i+1];
				playerInventory.itemLevelReq[      i]=playerInventory.itemLevelReq[      i+1];
				playerInventory.itemGenderReq[     i]=playerInventory.itemGenderReq[     i+1];
				playerInventory.itemClassReq[      i]=playerInventory.itemClassReq[      i+1];
				playerInventory.itemProficiencyReq[i]=playerInventory.itemProficiencyReq[i+1];
			}//end if
		} //end for
		redraw=true;drawDropPanel(); //refresh the drop panel now that the item is dropped. Make sure not to save the context on accident (redraw=true)
	}else{
		textOutput("<font color='FF5555'>Nothing there to drop.</font>");
	} //end if
} //end function
function tick(){
	player.cMove++;
	moveMobiles();
} //end function
