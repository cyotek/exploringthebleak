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
function listen(){
	setInterval(function(){
		var keys=KeyboardJS.activeKeys();
		//Asynchronous programming, we'll disable input while we operate on last input
		KeyboardJS.disable(); 
		if(keys=='a'       || keys.indexOf('num4')>-1){operate('west');
		}else if(keys=='x' || keys.indexOf('num2')>-1){operate('south');
		}else if(keys=='d' || keys.indexOf('num6')>-1){operate('east');
		}else if(keys=='w' || keys.indexOf('num8')>-1){operate('north');
		}else if(keys=='q' || keys.indexOf('num7')>-1){operate('northwest');
		}else if(keys=='e' || keys.indexOf('num9')>-1){operate('northeast');
		}else if(keys=='z' || keys.indexOf('num1')>-1){operate('southwest');
		}else if(keys=='c' || keys.indexOf('num3')>-1){operate('southeast');
		} //end if
		//Operate on last input is finished, re-enable input
		KeyboardJS.enable();
	},10);	
} //end function