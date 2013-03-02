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
function moveMobiles(){
	var numberOfMobs = 1+Math.floor(size/4),curMob=1;
	var direction=0,curX=0,curY=0;
	var moveMob=function(curMob,x,y,dx,dy){
		mobMap[x][y]=0; //clear mobmap of current position
		mobs[curMob].x=dx;mobs[curMob].y=dy; //move to new position
		mobMap[dx][dy]=mobs[curMob].num; //place curmob onto mobmap
		drawLocation(x,y);drawLocation(dx,dy);
	}; //end moveMob
	var locationMoveable=function(x,y){return (map[x][y]>=2 && mobMap[x][y]==0 && !isPlayerLoc(x,y)?true:false);};
	do{
		if (mobs[curMob].hp==0) {curMob++;continue;}
		curX=mobs[curMob].x;
		curY=mobs[curMob].y;
		if (map[curX][curY]<2){ //the mobile is not visible to the player
			direction=Math.floor(Math.random()*8);
			switch(direction){
			/* west */      case 0: if (curX>0 && mobs[curMob].hp>0){if (locationMoveable(curX-1,curY)){
						moveMob(curMob,curX,curY,curX-1,curY);}}break;
			/* east */      case 1: if (curX<size && mobs[curMob].hp>0){if (locationMoveable(curX+1,curY)){
						moveMob(curMob,curX,curY,curX+1,curY);}}break;
			/* north */     case 2: if (curY>0 && mobs[curMob].hp>0){if (locationMoveable(curX,curY-1)){
						moveMob(curMob,curX,curY,curX,curY-1);}}break;
			/* south */     case 3: if (curY<size && mobs[curMob].hp>0){if (locationMoveable(curX,curY+1)){
						moveMob(curMob,curX,curY,curX,curY+1);}}break;
			/* Northwest */ case 4: if (curX>0 && curY>0 && mobs[curMob].hp>0){if (locationMoveable(curX-1,curY-1)){
						moveMob(curMob,curX,curY,curX-1,curY-1);}}break;
			/* Northeast */ case 5: if (curX<size && curY>0 && mobs[curMob].hp>0){if (locationMoveable(curX+1,curY-1)){
						moveMob(curMob,curX,curY,curX+1,curY-1);}}break;
			/* Southwest */ case 6: if (curX>0 && curY>0 && mobs[curMob].hp>0){if (locationMoveable(curX-1,curY-1)){
						moveMob(curMob,curX,curY,curX-1,curY-1);}}break;
			/* Southeast */ case 7: if (curX<size && curY<size && mobs[curMob].hp>0){if (locationMoveable(curX+1,curY+1)){
						moveMob(curMob,curX,curY,curX+1,curY+1);}}break;
			} //end switch
		} else if (!touchPlayer(curMob,curX,curY)){ //the mobile is visible to the player
			/* favor cardinal directions over diagonal */
			/* northwest */	if(curX> player.mapPos.x && curY> player.mapPos.y && locationMoveable(curX-1,curY-1) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX-1,curY-1);
			/* northeast */	} else if(curX< player.mapPos.x && curY> player.mapPos.y && locationMoveable(curX+1,curY-1) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX+1,curY-1);
			/* southwest */	} else if(curX> player.mapPos.x && curY< player.mapPos.y && locationMoveable(curX-1,curY+1) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX-1,curY+1);
			/* southeast */	} else if(curX< player.mapPos.x && curY< player.mapPos.y && locationMoveable(curX+1,curY+1) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX+1,curY+1);
			/* north */	} else if(curY> player.mapPos.y && locationMoveable(curX,curY-1) && mobs[curMob].hp>0){ 
						moveMob(curMob,curX,curY,curX,curY-1);
			/* south */	} else if(curY< player.mapPos.y && locationMoveable(curX,curY+1) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX,curY+1);
			/* west */	} else if(curX> player.mapPos.x && locationMoveable(curX-1,curY) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX-1,curY);
			/* east */	} else if(curX< player.mapPos.x && locationMoveable(curX+1,curY) && mobs[curMob].hp>0){
						moveMob(curMob,curX,curY,curX+1,curY);
					} //end if
		} //end if
		curMob++;
	} while (curMob<numberOfMobs)
} //end function
function touchPlayer(curMob,curX,curY){
	/* northwest */	if(       isPlayerLoc(curX-1,curY-1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* northeast */	} else if(isPlayerLoc(curX+1,curY-1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* southwest */	} else if(isPlayerLoc(curX-1,curY+1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* southeast */	} else if(isPlayerLoc(curX+1,curY+1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* north */	    } else if(isPlayerLoc(curX  ,curY-1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* south */	    } else if(isPlayerLoc(curX  ,curY+1) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* west */	    } else if(isPlayerLoc(curX-1,curY  ) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
	/* east */	    } else if(isPlayerLoc(curX+1,curY  ) && mobs[curMob].hp>0 && map[curX][curY]==2){drawMob(curX,curY,curMob);attackPlayer(curMob);return true;
			        } //end if
	return false;
} //end function
function attackPlayer(curMob){
	var damage=0;
	if (       mobs[curMob].dam=="1d2" ){damage=1+Math.floor(Math.random()*2 );
	} else if (mobs[curMob].dam=="1d4" ){damage=1+Math.floor(Math.random()*4 );
	} else if (mobs[curMob].dam=="1d6" ){damage=1+Math.floor(Math.random()*6 );
	} else if (mobs[curMob].dam=="1d8" ){damage=1+Math.floor(Math.random()*8 );
	} else if (mobs[curMob].dam=="1d10"){damage=1+Math.floor(Math.random()*10);
	} else if (mobs[curMob].dam=="1d12"){damage=1+Math.floor(Math.random()*12);
	} else if (mobs[curMob].dam=="1d20"){damage=1+Math.floor(Math.random()*20);}
	textOutput("<font color='FF5555'>"+mobs[curMob].nam+"</font> hits you for <font color='FF5555'>"+damage+"</font> ("+mobs[curMob].dam+") damage.");
	player.chp-=damage;
	if (player.chp<=0){
		player.chp=0;
		textOutput("You die.");
		drawGameOver();
	} //end if
	drawHP();
} //end function
function hitMob(x,y){
	var damage=0;
	if (       player.baseDamage=="1d2" ){damage=1+Math.floor(Math.random()*2 );
	} else if (player.baseDamage=="1d4" ){damage=1+Math.floor(Math.random()*4 );
	} else if (player.baseDamage=="1d6" ){damage=1+Math.floor(Math.random()*6 );
	} else if (player.baseDamage=="1d8" ){damage=1+Math.floor(Math.random()*8 );
	} else if (player.baseDamage=="1d10"){damage=1+Math.floor(Math.random()*10);
	} else if (player.baseDamage=="1d12"){damage=1+Math.floor(Math.random()*12);
	} else if (player.baseDamage=="1d20"){damage=1+Math.floor(Math.random()*20);}
	mobs[mobMap[x][y]].hp-=damage;if (mobs[mobMap[x][y]].hp<0)mobs[mobMap[x][y]].hp=0;
	if (mobs[mobMap[x][y]].hp==0){
		textOutput("You punch and kill a <font color='FF5555'>"+mobs[mobMap[x][y]].nam+
				   "</font>.<br /><font color='559955'>You gained "+mobDB.hit[mobs[mobMap[x][y]].type]+
				   " experience.</font>");
		player.cxp+=parseInt(mobDB.hit[mobs[mobMap[x][y]].type]);if (player.cxp>=player.mxp)playerLevel();
		deadMap[x][y]=1;mobMap[x][y]=0;
		drawLocation(x,y);
		drawXP();
	} else {
		textOutput("You punch a <font color='FF5555'>"+mobs[mobMap[x][y]].nam+"</font>"+
				   " (<font color='FFFF55'>"+Math.floor(100/mobDB.hit[mobs[mobMap[x][y]].type]*mobs[mobMap[x][y]].hp)+"</font>% hp)"+
				   " for <font color='55FF55'>"+damage+"</font> ("+player.baseDamage+") damage.");
	} //end if
} //end function
function playerLevel(){
	player.cxp-=player.mxp;player.mxp+=25;player.level++;
	textOutput("<font color='55FF55'>Welcome to level "+player.level+"!</font>");
} //end function
