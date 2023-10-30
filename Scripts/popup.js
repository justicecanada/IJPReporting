/***************************/
//@Author: Adrian "yEnS" Mato Gondelle
//@website: www.yensdesign.com
//@email: yensamg@gmail.com
//@license: Feel free to use it, but keep this credits please!					
/***************************/



//loading popup with jQuery magic!
function loadPopup(){
	$("#backgroundPopup").css({
		"opacity": "0.7"
	});
	$("#backgroundPopup").fadeIn(300);
	$("#popupBet").fadeIn(300);
	$('html, body').animate({ scrollTop: 0 }, 'slow');
}

function loadBackground(){
	$("#backgroundPopup").css({
		"opacity": "0.7"
	});
	$("#backgroundPopup").fadeIn(100);
}

//disabling popup with jQuery magic!
function disablePopup(){
	$("#backgroundPopup").fadeOut("slow");
	$("#popupBet").fadeOut("slow");
}

//centering popup
function centerPopup(){
	//request data for centering
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#popupBet").height();
	var popupWidth = $("#popupBet").width();
	//centering
	$("#popupBet").css({
		"position": "absolute",
		"top": windowHeight/2-popupHeight/2,
		"left": windowWidth/2-popupWidth/2
	});
	//only need force for IE6
	
	$("#backgroundPopup").css({
		"height": windowHeight
	});
	
}

//CONTROLLING EVENTS IN jQuery
$(document).ready(function(){
	
	//LOADING POPUP
	//Click the button event!
	/*$(".Edit").click(function(){
		//centering with css
		centerPopup();
		//load popup
		loadPopup();
	});*/
	
	
	//CLOSING POPUP
	//Click the x event!
	$("#popupBetClose").click(function(){
	    disablePopup();
	});
	
	//Click out event!
	$("#backgroundPopup").click(function(){
	    disablePopup();
	});
	//Press Escape event!
	$(document).keypress(function(e){
		if(e.keyCode==27){
		    disablePopup();
		}
	});

});