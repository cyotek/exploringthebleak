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
function generateMobs(){
	/* Main Mapping Variables & declarations */
	var numberOfMobs = 1+Math.floor(size/4)
	var curMob=1;
	var xPos,yPos=0,randomMob;
	do {
		xPos=Math.floor(Math.random()*size+1);
		yPos=Math.floor(Math.random()*size+1);
		randomMob=Math.floor(Math.random()*mobDB.max)
		if (map[xPos][yPos]==1 && mobMap[xPos][yPos]==0 && !isPlayerLoc(xPos,yPos)){ //acceptable position for mobile
			mobMap[xPos][yPos]=curMob;
			mobs[curMob] ={
				x:xPos,
				y:yPos,
				hp:mobDB.hit[randomMob],
				num:curMob,
				type:randomMob,
				nam:mobDB.nam[randomMob],
				hei:mobDB.hei[randomMob],
				sym:mobDB.sym[randomMob],
				dam:mobDB.dam[randomMob],
				des:mobDB.des[randomMob],
				tileSet:mobDB.tileSet[randomMob],
				tilePos:{
					x:mobDB.tilePos.x[randomMob],
					y:mobDB.tilePos.y[randomMob]
				} //end tilePos
			}; //end mobs
			//textOutput(mobs[curMob].hp);
			curMob++;
		} //end if
	} while (curMob<numberOfMobs)
} //end function