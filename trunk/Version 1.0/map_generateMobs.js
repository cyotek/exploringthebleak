function generateMobs(){
	var cur=0;
	var rx=0,ry=0;
	do{
		rx=Math.floor(Math.random()*size);
		ry=Math.floor(Math.random()*size);
		/* make sure it's walkable,not the players position, and isn't populated by any other mobs */
		if(map[rx][ry].type==tileDirtFloor && rx!=cx ||map[rx][ry].type==tileDirtFloor && ry!=cy && mobMap[rx][ry]==0){ 
			mobMap[rx][ry]=cur;
			mobs[cur].location.x=rx;
			mobs[cur].location.y=ry;
			cur++;
		}
	}while(cur<Math.floor(size/2))
} //end function