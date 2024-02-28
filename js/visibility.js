function setVisibility(objectID,state) {
	var dom = findDOM(objectID,1);
	dom.visibility = state;
	if (state == 'visible') {
		dom.display = 'block';
	}
	else {
		dom.display = 'none';
	}
}

function toggleVisiblity(objectID) {
	var dom = findDOM(objectID,1);
	state = dom.visibility;
	if (state == 'hidden' || state =='hide') {
		dom.visibility = 'visible';
		dom.display = 'block';
	}
	else {
		if (state == 'visible' || state == 'show') {
			dom.visibility = 'hidden';
			dom.display = 'none';
		}
		else {
			dom.visibility = 'visible';
			dom.display = 'block';
		}	
	}
}
