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
/* generic constructor variables to keep track of */
var __localPlaneGeometry; //save the geometry dimensions of the plane
var __localPlaneMaterial; //save the material specific to the plane
var __localBlockGeometry; //save the geometry dimensions of the block
var __localBlockMaterial; //save the material specific to the block

/* Save and load the object on demand to prevent unnecessary redimensionalization of geometry */
function savPlane(w,h,texture){
	__localPlaneGeometry= new THREE.PlaneGeometry(w,h);
	__localPlaneGeometry.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	__localPlaneMaterial= new THREE.MeshLambertMaterial({map:texture});
} //end function
function savTPlane(w,h,texture){
	__localPlaneGeometry= new THREE.PlaneGeometry(w,h);
	__localPlaneGeometry.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	__localPlaneMaterial= new THREE.MeshLambertMaterial({map:texture});
	__localPlaneMaterial.transparent=true;
} //end function
function repPlaneT(texture,transparent){
	__localPlaneMaterial= new THREE.MeshLambertMaterial({map:texture});
	if(transparent==true){
		__localPlaneMaterial.transparent=true;
	} //end if
} //end function
function runPlane(x,y,z){
	var _p = new THREE.Mesh(__localPlaneGeometry,__localPlaneMaterial);
	_p.position.set(x,y,z);
	scene.add(_p);
} //end function
function savBlock(w,h,d,texture){
	__localBlockGeometry= new THREE.CubeGeometry(w,d,h);
	__localBlockGeometry.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	__localBlockMaterial= new THREE.MeshLambertMaterial({map:texture});
} //end function
function repBlockT(texture){
	__localBlockMaterial= new THREE.MeshLambertMaterial({map:texture});
} //end function
function runBlock(x,y,z){
	var _b = new THREE.Mesh(__localBlockGeometry,__localBlockMaterial);
	_b.position.set(x,y,z);
	scene.add(_b);
} //end function

/* create primitive geometry from scratch */
function addPlane(x,y,z,w,h,texture){
	var _g = new THREE.PlaneGeometry(w,h);
	    _g.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	var _m = new THREE.MeshLambertMaterial({map:texture});
	var _p = new THREE.Mesh(_g,_m);
	    _p.position.set(x,y,z);
	scene.add(_p);
} //end function
function addTPlane(x,y,z,w,h,texture){ //transparent allowant plane
	var _g = new THREE.PlaneGeometry(w,h);
	    _g.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	var _m = new THREE.MeshLambertMaterial({map:texture});
	    _m.transparent=true;
	var _p = new THREE.Mesh(_g,_m);
	    _p.position.set(x,y,z);
	scene.add(_p);
} //end function
function addBlock(x,y,z,w,h,d,texture){
	var _g = new THREE.CubeGeometry(w,d,h);
	    _g.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	var _m = new THREE.MeshLambertMaterial({map:texture});
	var _b = new THREE.Mesh(_g,_m);
	    _b.position.set(x,y,z);
	scene.add(_b);
} //end function
function addBlockCustom(x,y,z,w,h,d,texture1,texture2,texture3,texture4,texture5,texture6){
	var _g = new THREE.CubeGeometry(w,d,h);
	    _g.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	var _m = [];
	    _m.push(new THREE.MeshLambertMaterial({map:texture1}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture2}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture3}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture4}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture5}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture6}));
	for (var __u=0;__u<=5;__u++){
		_g.faces[__u].materialIndex=__u;
	} //end for
	var _b = new THREE.Mesh(_g,new THREE.MeshFaceMaterial(_m));
	    _b.position.set(x,y,z)
	scene.add(_b);
} //end function
function addBlockCustomAllExceptOne(x,y,z,w,h,d,texture1,texture2,face){
	var _g = new THREE.CubeGeometry(w,d,h);
	    _g.applyMatrix(new THREE.Matrix4().makeRotationX(-Math.PI/2));
	var _m = [];
	    _m.push(new THREE.MeshLambertMaterial({map:texture1}));
	    _m.push(new THREE.MeshLambertMaterial({map:texture2}));
	for (var __u=0;__u<=5;__u++){
		_g.faces[__u].materialIndex=(__u==face?0:1);
	} //end for
	var _b = new THREE.Mesh(_g,new THREE.MeshFaceMaterial(_m));
	    _b.position.set(x,y,z)
	scene.add(_b);
} //end function