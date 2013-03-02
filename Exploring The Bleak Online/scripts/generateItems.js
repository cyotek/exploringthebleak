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
function generateItems(){
	/* Main Mapping Variables & declarations */
	var numberOfItems = 1+Math.floor(size/4),curItem=1;
	var xPos,yPos=0,randomItem;
	do {
		xPos=Math.floor(Math.random()*size+1);
		yPos=Math.floor(Math.random()*size+1);
		randomItem=Math.floor(Math.random()*2); //find out whether we'll generate a weapon or armor
		items.curItem[curItem]=randomItem; //dictate in items whether it was armor or weapon
		if (randomItem==1){ //weapon
			randomItem=Math.floor(Math.random()*weaponDB.max)
			if (map[xPos][yPos]==1 && itemMap[xPos][yPos]==0 && !isPlayerLoc(xPos,yPos)){ //acceptable position for mobile
				items.weapons[curItem] ={
					x:          xPos,
					y:          yPos,
					name:       weaponDB.name[       randomItem],
					num:        curItem,
					type:       randomItem,
					character:  weaponDB.character[  randomItem],
					damagetype: weaponDB.damageType[ randomItem],
					description:weaponDB.description[randomItem],
					classReq:   weaponDB.classReq[   randomItem],
					damage:     weaponDB.damage[     randomItem],
					twoHanded:  weaponDB.twoHanded[  randomItem],
					levelReq:   weaponDB.levelReq[   randomItem],
					worth:      weaponDB.worth[      randomItem]
				}; //end weapon
				itemMap[xPos][yPos]=curItem;
				curItem++;
			} //end if
		} else { //armor
			randomItem=Math.floor(Math.random()*armorDB.max)
			if (map[xPos][yPos]==1 && itemMap[xPos][yPos]==0 && !isPlayerLoc(xPos,yPos)){ //acceptable position for mobile
				items.armor[curItem] ={
					x:             xPos,
					y:             yPos,
					name:          armorDB.name[          randomItem],
					num:           curItem,
					type:          randomItem,
					character:     armorDB.character[     randomItem],
					equipmentType: armorDB.equipmentType[ randomItem],
					proficiencyReq:armorDB.proficiencyReq[randomItem],
					levelReq:      armorDB.levelReq[      randomItem],
					genderReq:     armorDB.genderReq[     randomItem],
					bashAC:        armorDB.bashAC[        randomItem],
					slashAC:       armorDB.slashAC[       randomItem],
					pierceAC:      armorDB.pierceAC[      randomItem],
					fistAC:        armorDB.fistAC[        randomItem],
					exoticAC:      armorDB.exoticAC[      randomItem],
					worth:         armorDB.worth[         randomItem],
					weight:        armorDB.weight[        randomItem],
					description:   armorDB.description[   randomItem]
				}; //end armor
				itemMap[xPos][yPos]=curItem;
				curItem++;
			} //end if
		} //end if
	} while (curItem<numberOfItems)
} //end function