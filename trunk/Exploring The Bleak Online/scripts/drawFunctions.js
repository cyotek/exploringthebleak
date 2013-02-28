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
function drawMap(){
	/* Set the floor color. Put inside loop to ensure it's never too dark*/
	pickLightColor();
	/* If the ui hasn't been drawn yet, then draw it now */
	if (initialized==0){
		/* push the text pad to it's correct location */
		var pad = document.getElementById("pad")
		pad.style.backgroundColor='black';
		pad.style.left=5+size*roomsize+"px";
		pad.style.top=3+"px";
		pad.style.width=canvasWidth-14-size*roomsize+"px";
		pad.style.height=canvasHeight/2+"px";
		pad.style.padding=3+"px";
		pad.style.overflow="scroll";
		/* fill the canvas background to black */
		context.fillStyle='#000';
		context.fillRect(1,1,canvasWidth,y*roomsize-roomsize+3);
		/* fill the map background to dark gray */
		context.fillStyle='#080808';
		context.fillRect(1,1,x*roomsize-roomsize+2,y*roomsize-roomsize+2);
		/* draw the hp,wp, & xp bars */
		drawHP();
		drawWP();
		drawXP();
		/* outline the map */
		context.strokeStyle='#444';
		context.lineWidth=3;
		context.strokeRect(1,1,x*roomsize-roomsize+3,y*roomsize-roomsize+3);
		context.strokeRect(1,1,canvasWidth-2,y*roomsize-roomsize+3);
		initialized=1;
	}
	/* place the player and draw FOV*/
	playerFOV(player.fieldOfViewRange,player.mapPos.x,player.mapPos.y,1); 
	drawPlayer();
} //end function
function pickLightColor(){
	playerColor=upColor(environment.color.charAt(0))+upColor(environment.color.charAt(1))+upColor(environment.color.charAt(2))+
				upColor(environment.color.charAt(3))+upColor(environment.color.charAt(4))+upColor(environment.color.charAt(5));
} //end function
function upColor(color){
     if (color=='0'){color='2';} else if (color=='1'){color='3';} else if (color=='2'){color='4';}
else if (color=='3'){color='5';} else if (color=='4'){color='6';} else if (color=='5'){color='7';}
else if (color=='6'){color='8';} else if (color=='7'){color='9';} else if (color=='8'){color='A';}
else if (color=='9'){color='B';} else if (color=='A'){color='C';} else if (color=='B'){color='D';}
else if (color=='C'){color='E';} else if (color=='D'){color='F';} else if (color=='E'){color='F';}
return color;
} //end function
function drawHP(){
	var fullBar=canvasHeight/2-(canvasHeight-size*roomsize+11);
	var filledBar=fullBar/player.mhp*player.chp;
	var notFilledBar=fullBar/player.mhp*(player.mhp-player.chp);
	var barWidth=(canvasWidth-14-size*roomsize)/3-1;
	var xBar=(size+1)*roomsize-roomsize+7;
	var yBar=canvasHeight/2+13;
	context.fillStyle='#400404';context.strokeStyle='#000';context.lineWidth=1;
	context.strokeRect(xBar,yBar,barWidth,fullBar);
	context.fillRect(xBar,yBar,barWidth,fullBar);
	context.fillStyle='#800808';
	context.fillRect(xBar,notFilledBar+yBar,barWidth,filledBar);
	context.save();
	context.rotate(-Math.PI/2);
	context.fillStyle='white';
	context.font=roomsize-3+"px arial";context.textAlign="center";context.lineWidth=4;
	context.strokeText(player.chp+" / "+player.mhp+" HP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.fillText(player.chp+" / "+player.mhp+" HP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.restore();
} //end function
function drawWP(){
	var fullBar=canvasHeight/2-(canvasHeight-size*roomsize+11);
	var filledBar=fullBar/player.mwp*player.cwp;
	var notFilledBar=fullBar/player.mwp*(player.mwp-player.cwp);
	var barWidth=(canvasWidth-14-size*roomsize)/3-1;
	var xBar=(size+1)*roomsize-roomsize+9+barWidth;
	var yBar=canvasHeight/2+13
	context.fillStyle='#040440';context.strokeStyle='#000';context.lineWidth=1;
	context.strokeRect(xBar,yBar,barWidth,fullBar);
	context.fillRect(xBar,yBar,barWidth,fullBar);
	context.fillStyle='#080880';
	context.fillRect(xBar,notFilledBar+yBar,barWidth,filledBar);
	context.save();
	context.rotate(-Math.PI/2);
	context.fillStyle='white';
	context.font=roomsize-3+"px arial";context.textAlign="center";context.lineWidth=4;
	context.strokeText(player.cwp+" / "+player.mwp+" WP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.fillText(player.cwp+" / "+player.mwp+" WP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.restore();
} //end function
function drawXP(){
	var fullBar=canvasHeight/2-(canvasHeight-size*roomsize+11);
	var filledBar=fullBar/player.mxp*player.cxp;
	var notFilledBar=fullBar/player.mxp*(player.mxp-player.cxp);
	var barWidth=(canvasWidth-14-size*roomsize)/3-1;
	var xBar=(size+1)*roomsize-roomsize+11+barWidth*2;
	var yBar=canvasHeight/2+13
	context.fillStyle='#044004';context.strokeStyle='#000';context.lineWidth=1;
	context.strokeRect(xBar,yBar,barWidth,fullBar);
	context.fillRect(xBar,yBar,barWidth,fullBar);
	context.fillStyle='#088008';
	context.fillRect(xBar,notFilledBar+yBar,barWidth,filledBar);
	context.save();
	context.rotate(-Math.PI/2);
	context.fillStyle='white';
	context.font=roomsize-3+"px arial";context.textAlign="center";context.lineWidth=4;
	context.strokeText(player.cxp+" / "+player.mxp+" XP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.fillText(player.cxp+" / "+player.mxp+" XP",-yBar-fullBar/2,xBar+barWidth/2+roomsize/2-3);
	context.restore();
} //end function
function drawLocation(x,y){
	var firstRun=0;
	do{
		if (map[x][y]==2){
			drawLightFloor(x,y);
			if (deadMap[x][y]==1){ //dead mobile here
				drawCircleSector(x,y,'#800','#000');
			} //end if
			/* check to see if the current sector is stairs up or down and draw it underneath
			   the mobile (or player) and above the floor color */
			if (x==stairsUpX && y==stairsUpY){
				if (!graphics){drawASCII(x,y,"<",'#4A4');
				}else{drawImage(x,y,stairuImg);} //stairs up
			} else if (x==stairsDownX && y==stairsDownY){
				if (!graphics){drawASCII(x,y,"<",'#4A4');
				}else{drawImage(x,y,stairdImg);} //stairs down
			} //end if
			/* draw the item */
			if (itemMap[x][y]>=1){
				if (items.curItem[itemMap[x][y]]==1){ //weapon
					if (!graphics){drawASCII(x,y,items.weapons[itemMap[x][y]].character,'#990');
					}else{drawImage(x,y,chestCImg);} //closed chest image
				} else { //armor
					if (!graphics){drawASCII(x,y,items.armor[itemMap[x][y]].character,'#909');
					}else{drawImage(x,y,chestCImg);} //closed chest image
				} //end if
			} //end if
			/* draw the mob */
			if (mobMap[x][y]>=1){
				drawMob(x,y,mobMap[x][y]);
			} //end if
			/* draw the player ontop of everything else */
			if (x==player.mapPos.x && y==player.mapPos.y){
				drawPlayer()
			} //end if
		} else if (map[x][y]==3){
			if (!graphics){drawDarkFloor(x,y);}else{drawLightFloor(x,y);}
			if (deadMap[x][y]==1){ //dead mobile here
				drawCircleSector(x,y,'#300','#000');
			} //end if
			/* check to see if the current sector is stairs up or down and draw it underneath
			   the mobile (or player) and above the floor color */
			if (x==stairsUpX && y==stairsUpY){
				if (!graphics){drawASCII(x,y,"<",'#151');
				}else{drawImage(x,y,stairuImg);} //stairs up image
			} else if (x==stairsDownX && y==stairsDownY){
				if (!graphics){drawASCII(x,y,"<",'#151');
				}else{drawImage(x,y,stairdImg);} //stairs down image
			} //end if
		    /* draw the item */
			if (itemMap[x][y]>=1){
				if (items.curItem[itemMap[x][y]]==1){ //weapon
					if (!graphics){drawASCII(x,y,items.weapons[itemMap[x][y]].character,'#440');
					}else{drawImage(x,y,chestCImg);} //closed chest image
				} else { //armor
					if (!graphics){drawASCII(x,y,items.armor[itemMap[x][y]].character,'#404');
					}else{drawImage(x,y,chestCImg);} //closed chest image
				} //end iff
			} //end if
			if (graphics)drawImage(x,y,shadow04Img); //draw a shadow over sector if it's graphics
			if (graphics)drawImage(x,y,shadow04Img); //draw a shadow over sector if it's graphics
		} //end if
		firstRun++;y<size?y++:firstRun++; //draw the tile below (because of character size being too large
	} while (firstRun<=1)
} //end function
function drawLightFloor(x,y){
	context.fillStyle=playerColor;
	context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize,roomsize);
	drawSectorShadows(x,y);
} //end function
function drawDarkFloor(x,y){
	context.fillStyle=environment.color;
	context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize,roomsize);
	drawSectorShadows(x,y);
} //end function
function isShadowWall(x,y){
	if (wallMap[x][y]==7||wallMap[x][y]==6||wallMap[x][y]==2||wallMap[x][y]==1){
		return true;
	} else {return false;}
} //end function
function drawSectorShadows(x,y){
	if (graphics && x>0){
		if (map[x-1][y]==0){if (isShadowWall(x-1,y))drawImage(x,y,shadow03Img);} //shadow west side
	} //end if
	if (graphics && y>0){
		if (map[x][y-1]==0){if (isShadowWall(x,y-1))drawImage(x,y,shadow01Img);} //shadow north side
	} //end if
	if (graphics && x>0 && y>0){
		if (map[x-1][y]==0 && map[x][y-1]==0){
			if (isShadowWall(x-1,y))drawImage(x,y,shadow02Img);
		} else if (map[x-1][y]!=0 && map[x][y-1]!=0 && map[x-1][y-1]==0){
			if (isShadowWall(x-1,y-1)||isShadowWall(x-1,y)&&isShadowWall(x,y-1))drawImage(x,y,shadow02Img); //shadow northwest
		} //end if
	} //end if
} //end function
function drawASCII(x,y,text,color,offsetX,offsetY){
	/* draws an ascii character at a certain location on the map */
	if (offsetX==undefined)offsetX=0;if (offsetY==undefined)offsetY=0;
	context.font=roomsize-3+"px arial";context.textAlign="center";context.lineWidth=2;
	context.strokeStyle='black';context.strokeText(text,x*roomsize+roomsize/2-roomsize+3+offsetX,roomsize/6+y*roomsize+roomsize/2-roomsize+8+offsetY);
	context.fillStyle=color;context.fillText(text,x*roomsize+roomsize/2-roomsize+3+offsetX,roomsize/6+y*roomsize+roomsize/2-roomsize+8+offsetY);
} //end function
function drawSector(x,y,color){
	context.fillStyle=environmentColor;
	context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize+1,roomsize+1);
} //end function
function drawDiamondSector(x,y,color,strokeColor){
	var xPos=x*roomsize-roomsize+3,yPos=y*roomsize-roomsize+3;
	context.fillStyle=color;context.strokeStyle=strokeColor;context.lineWidth=1;
	context.beginPath();
	context.moveTo(xPos,yPos+roomsize/2);
	context.lineTo(xPos+roomsize/2,yPos);
	context.lineTo(xPos+roomsize,yPos+roomsize/2);
	context.lineTo(xPos+roomsize/2,yPos+roomsize);
	context.closePath();
	context.fill();
	context.stroke();
} //end function
function drawCircleSector(x,y,color,strokeColor){
	context.fillStyle=color;context.strokeStyle=strokeColor;context.lineWidth=1;
	context.beginPath();                                                                          
	context.arc(x*roomsize-roomsize+roomsize/2+3,y*roomsize-roomsize+roomsize/2+3,roomsize/2-2,0,2*Math.PI,false);
	context.fill();                                            
	context.stroke();    
} //end function
function drawLine(targetX,targetY){
	context.beginPath();
	context.moveTo(roomsize*player.mapPos.x-roomsize/2,roomsize*player.mapPos.y-roomsize/2);
	context.lineTo(roomsize*targetX-roomsize/2,roomsize*targetY-roomsize/2);
	context.stroke();
} //end function
function drawBlock(){
	canvasData = context.getImageData(1,1,canvasWidth,canvasHeight);
	context.strokeStyle='#4FF';
	context.lineWidth=2;
	context.strokeRect(cx*roomsize-roomsize+3,cy*roomsize-roomsize+3,roomsize+1,roomsize+1);
} //end function
function drawRoundRect(x, y, width, height, radius) {
	if (typeof radius === "undefined")radius = 5;
	context.beginPath();
	context.moveTo(x + radius, y);
	context.lineTo(x + width - radius, y);
	context.quadraticCurveTo(x + width, y, x + width, y + radius);
	context.lineTo(x + width, y + height - radius);
	context.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
	context.lineTo(x + radius, y + height);
	context.quadraticCurveTo(x, y + height, x, y + height - radius);
	context.lineTo(x, y + radius);
	context.quadraticCurveTo(x, y, x + radius, y);
	context.closePath();
	context.fillStyle='#000';
	context.fill();
} //end function
function drawPanel(width,height,text){
	/* Save the context before adding blur */
	if(redraw==false){context.save();}else{redraw=false;} //save the context if it's not a redraw
	context.globalAlpha = 0.50;
	/* save the current map to be redrawn after closing panel */
	curx=width;cury=height,x=0; //temporary variables
	canvasData = context.getImageData(1, 1, canvasWidth, canvasHeight);
	/* draw the shadow box */
	drawRoundRect((canvasMax-width)/2+10,(canvasHeight-500)/2+10,width,height);
	/* restore the context before blur and draw the inside and contents of panel */
	context.restore();
	context.fillStyle='#222';
	context.fillRect((canvasMax-width)/2,(canvasHeight-500)/2,width,height);
	context.strokeStyle='#444';
	context.strokeRect((canvasMax-width)/2,(canvasHeight-500)/2,width,height);
	context.font="12px arial";
	drawPanelTextBol(text);
} //end function
function drawPanelTextReg(text){
	context.textAlign="left";context.fillStyle='#999';x++;
	context.fillText(text,5+(canvasMax-curx)/2,15*x+(canvasHeight-cury)/2);
} //end function
function drawPanelTextBol(text){
	context.textAlign="center";context.fillStyle='#FFF';x++;
	context.fillText(text,(canvasMax-curx)/2+curx/2,15*x+(canvasHeight-cury)/2);
} //end function
function drawCharacterPanel(){
	drawPanel(200,500,'Character');
	drawPanelTextReg("Name:  "+player.pname );
	drawPanelTextReg("Race:  "+player.prace );
	drawPanelTextReg("Class: "+player.pclass);
	drawPanelTextReg("Level: "+player.level);
	drawPanelTextBol('Condition');
	drawPanelTextReg(player.chp+" / "+player.mhp+" HP");
	drawPanelTextReg(player.cwp+" / "+player.mwp+" WP");
	drawPanelTextReg(player.cxp+" / "+player.mxp+" XP");
	drawPanelTextBol('Core Stats');
	drawPanelTextReg(player.stats.strength+    " / "+player.stats.strengthMax+    " STR");
	drawPanelTextReg(player.stats.dexterity+   " / "+player.stats.dexterityMax+   " DEX");
	drawPanelTextReg(player.stats.intelligence+" / "+player.stats.intelligenceMax+" INT");
	drawPanelTextReg(player.stats.wisdom+      " / "+player.stats.wisdomMax+      " WIS");
	drawPanelTextReg(player.stats.constitution+" / "+player.stats.constitutionMax+" CON");
	drawPanelTextReg(player.stats.charisma+    " / "+player.stats.charismaMax+    " CHA");
	drawPanelTextReg(player.stats.luck+        " / "+player.stats.luckMax+        " LUC");
} //end function
function drawDropPanel(){
	drawPanel(200,500,'Drop Item');
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
} //end function
function clearPanel(){
	context.putImageData(canvasData, 1, 1);
} //end function
function drawPlayer(){
	/* the player is drawn as an '@' symbol */
	if (!graphics){drawASCII(player.mapPos.x,player.mapPos.y,'@','#0F0');
	}else{
		if (player.prace=="Elf"||player.prace=="Half-elf"||player.prace=="Halfling"||player.prace=="Orc"){ //tileset 'classm'
			drawTileImage(player.mapPos.x,player.mapPos.y,player.tilePos.x,player.tilePos.y,monster01Img);
		} else if (player.prace=="Goblin"||player.prace=="Troll"||player.prace=="Half-orc"||player.prace=="Human"){ //tileset monster1
			drawTileImage(player.mapPos.x,player.mapPos.y,player.tilePos.x,player.tilePos.y,monster05Img);
		} else if (player.prace=="Quickling"||player.prace=="Pixie"||player.prace=="Sprite"){ //tileset monster2
			drawTileImage(player.mapPos.x,player.mapPos.y,player.tilePos.x,player.tilePos.y,monster06Img);
		} else if (player.prace=="Kobold"){ //tileset monster3
			drawTileImage(player.mapPos.x,player.mapPos.y,player.tilePos.x,player.tilePos.y,monster07Img);
		} else if (player.prace=="Dwarf"||player.prace=="Gnome"){ //tileset monster6
			drawTileImage(player.mapPos.x,player.mapPos.y,player.tilePos.x,player.tilePos.y,monster10Img);
		} //end if
	} //end if
} //end function
function drawMob(xPos,yPos,curMob){
	if (!graphics){drawASCII(xPos,yPos,mobs[curMob].sym,'#900');
	}else{
		if (mobs[curMob].tileSet=='classm'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster01Img);
		} else if (mobs[curMob].tileSet=='dragon'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster02Img);
		} else if (mobs[curMob].tileSet=='undead'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster03Img);
		} else if (mobs[curMob].tileSet=='unique'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster04Img);
		} else if (mobs[curMob].tileSet=='monster1'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster05Img);
		} else if (mobs[curMob].tileSet=='monster2'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster06Img);
		} else if (mobs[curMob].tileSet=='monster3'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster07Img);
		} else if (mobs[curMob].tileSet=='monster4'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster08Img);
		} else if (mobs[curMob].tileSet=='monster5'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster09Img);
		} else if (mobs[curMob].tileSet=='monster6'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster10Img);
		} else if (mobs[curMob].tileSet=='monster7'){
			drawTileImage(xPos,yPos,mobs[curMob].tilePos.x,mobs[curMob].tilePos.y,monster11Img);
		} //end if
	} //end if
} //end function
function drawOutlineSector(x,y,color,strokeColor){
	context.fillStyle=color;context.strokeStyle=strokeColor;context.lineWidth=1;
	context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize,roomsize);
	context.strokeRect(x*roomsize-roomsize+4,y*roomsize-roomsize+4,roomsize-2,roomsize-2);
} //end function
function drawTileImage(x,y,xNum,yNum,imageSrc){
	context.drawImage(imageSrc,xNum*32,yNum*32,32,32,x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize,roomsize);
} //end function
function drawImage(x,y,imageSrc){
	context.drawImage(imageSrc,x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize,roomsize);
} //end function
function drawWall(x,y,visible){
	if (visible==1){
			if (wallMap[x][y]==0){ 
				if (!graphics){drawLightFloor(x,y);drawDiamondSector(x,y,'#330','#550');drawASCII(x,y,"f",'#222',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall07Img);} //draw ugly tree image
				wall=true;
			} else if (wallMap[x][y]==1){
				if (!graphics){drawOutlineSector(x,y,'#003','#005');drawASCII(x,y,"w",'#222',-1,-3)
				}else{drawLightFloor(x,y);drawImage(x,y,wall08Img);} //draw water block image
				wall=true;
			} else if (wallMap[x][y]==2){
				if (!graphics){drawOutlineSector(x,y,'#330','#550');drawASCII(x,y,"o",'#222',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall01Img);} //dirt block image
				wall=true;
			} else if (wallMap[x][y]==3){
				if (!graphics){drawLightFloor(x,y);drawCircleSector( x,y,'#333','#555');drawASCII(x,y,"r",'#222',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall03Img);} //rock image
				wall=true;
			} else if (wallMap[x][y]==4){
				if (!graphics){drawLightFloor(x,y);drawDiamondSector(x,y,'#030','#050');drawASCII(x,y,"s",'#222',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall06Img);} //tree tall image
				wall=true;
			} else if (wallMap[x][y]==5){
				if (!graphics){drawLightFloor(x,y);drawCircleSector( x,y,'#030','#050');drawASCII(x,y,"t",'#222',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall05Img);} //tree short image
				wall=true;
			} else if (wallMap[x][y]==6){
				if (!graphics){drawOutlineSector(x,y,'#300','#500');drawASCII(x,y,"L",'#222',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall02Img);} //lava block image
				wall=true;
			} else if (wallMap[x][y]==7){
				if (!graphics){drawOutlineSector(x,y,'#555','#000');drawASCII(x,y,"#",'#222',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall04Img);} //stone block tall image
				wall=true;}
	} else { //not visible (faded out)
			if (wallMap[x][y]==0){
				if (!graphics){drawDarkFloor(x,y);drawDiamondSector(x,y,'#110','#330');drawASCII(x,y,"f",'#111',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall07Img);} //draw ugly tree image
				wall=true;
			} else if (wallMap[x][y]==1){
				if (!graphics){drawOutlineSector(x,y,'#001','#003');drawASCII(x,y,"w",'#111',-1,-3)
				}else{drawLightFloor(x,y);drawImage(x,y,wall08Img);} //draw water block image
				wall=true;
			} else if (wallMap[x][y]==2){
				if (!graphics){drawOutlineSector(x,y,'#110','#330');drawASCII(x,y,"o",'#111',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall01Img);} //dirt block image
				wall=true;
			} else if (wallMap[x][y]==3){
				if (!graphics){drawDarkFloor(x,y);drawCircleSector( x,y,'#111','#333');drawASCII(x,y,"r",'#111',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall03Img);} //rock image
				wall=true;
			} else if (wallMap[x][y]==4){
				if (!graphics){drawDarkFloor(x,y);drawDiamondSector(x,y,'#010','#030');drawASCII(x,y,"s",'#111',-1,-3);
				}else{drawLightFloor(x,y);drawImage(x,y,wall06Img);} //tree tall image
				wall=true;
			} else if (wallMap[x][y]==5){
				if (!graphics){drawDarkFloor(x,y);drawCircleSector( x,y,'#010','#030');drawASCII(x,y,"t",'#111',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall05Img);} //tree short image
				wall=true;
			} else if (wallMap[x][y]==6){
				if (!graphics){drawOutlineSector(x,y,'#100','#300');drawASCII(x,y,"L",'#111',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall02Img);} //lava block image
				wall=true;
			} else if (wallMap[x][y]==7){
				if (!graphics){drawOutlineSector(x,y,'#333','#000');drawASCII(x,y,"#",'#111',-1,-1);
				}else{drawLightFloor(x,y);drawImage(x,y,wall04Img);} //stone block tall image
				wall=true;}
			if (graphics)drawImage(x,y,shadow04Img); //draw a shadow over sector if it's graphics
			if (graphics)drawImage(x,y,shadow04Img); //draw a shadow over sector if it's graphics
	} //end if
} //end function

