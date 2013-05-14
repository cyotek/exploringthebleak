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
/* ensure that the location intended is walkable first */
function walkable(x,y){
	var t=map[x][y].type;
	return (t==tileDirtFloor?true:(t==tileCorridor?true:(t==tileDoor?true:(t==tileUpStairs?true:(t==tileDownStairs?true:false)))));
} //end function
/* process and operate on the command the user inputted */
function operate(command){
	if(command=="west" && walkable(cx-1,cy)){
		cx--;playerObject.position.x-=10;
	}else if(command=="east" && walkable(cx+1,cy)){
		cx++;px=-10;playerObject.position.x+=10;
	}else if(command=="north" && walkable(cx,cy-1)){
		cy--;py=-10;playerObject.position.z-=10;
	}else if(command=="south" && walkable(cx,cy+1)){
		cy++;py=10;playerObject.position.z+=10;
	}else if(command=="northeast" && walkable(cx+1,cy-1)){
		cx++;cy--;playerObject.position.z-=10;playerObject.position.x+=10;px=cx;py=cy;move();
	}else if(command=="northwest" && walkable(cx-1,cy-1)){
		cx--;cy--;playerObject.position.z-=10;playerObject.position.x-=10;px=cx;py=cy;move();
	}else if(command=="southwest" && walkable(cx-1,cy+1)){
		cx--;cy++;playerObject.position.z+=10;playerObject.position.x-=10;px=cx;py=cy;move();
	}else if(command=="southeast" && walkable(cx+1,cy+1)){
		cx++;cy++;playerObject.position.z+=10;playerObject.position.x+=10;px=cx;py=cy;move();
	} //end if
} //end function