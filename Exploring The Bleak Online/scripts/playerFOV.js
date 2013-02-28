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
function drawline_mod(x,y,x2,y2,type) {
    var dy = Math.abs(x - x2);
    var dx = Math.abs(y - y2);
    var s = .99/(dy>dx?dy:dx);
    var t = 0.0;
    while(t < 1.0) {
        dy = Math.floor((1.0 - t)*x + t*x2);
        dx = Math.floor((1.0 - t)*y + t*y2);
        if (map[dx][dy] != 0) {
		if (type==1){
			visMap[dx][dy]=1;
			map[dx][dy]=2;
			drawLocation(dx,dy);
		} else if (type==0 && map[dx][dy]==2){
			visMap[dx][dy]=0;
			map[dx][dy]=3; //set the tile to previously visited but darkened
			drawLocation(dx,dy);
		} //end if
	} else {
		if (type==1){
			visMap[dx][dy]=1;
			drawWall(dx,dy,1);
		} else {
			visMap[dx][dy]=0;
			drawWall(dx,dy,0);
		} //end if
		return;
	} //end if
        t += s; 
    } //end while
} //end function
function playerFOV(range,ply,plx,type) {
	var x, y,f;
	for (f = 0.1; f < 3.14*2; f += 0.05) {
		x = Math.floor(range*Math.cos(f)) + plx;
		y = Math.floor(range*Math.sin(f)) + ply;
		drawline_mod(plx,ply,x+1,y+1,type);
	} //end for
} //end function

