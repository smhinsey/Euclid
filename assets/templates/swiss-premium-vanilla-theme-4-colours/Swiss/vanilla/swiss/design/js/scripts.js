// JavaScript Document

function discussionButton() {
	
	//gets the discussion button
	var button = jQuery('#categories > .NewDiscussion');
	
	//if it exists
	if(button.length > 0) {
		
		//erases it from ther and copies in its right place
		var buttonHTML = button.html();
		var buttonURL = button.attr('href');
		button.remove();
		
		jQuery('#Content .DiscussionsTabs > ul').append('<li class="add-discussion"><a href="'+buttonURL+'">'+buttonHTML+'</a></li>');
		
	}
	
	//gets the sign in button
	var signIn = jQuery('.SignInPopup');
	
	if(signIn.length > 0) {
		
		var signInText = signIn.html();
		var signInUrl = signIn.attr('href');
		
		jQuery('#Content .DiscussionsTabs > ul').append('<li class="add-discussion"><a href="'+signInUrl+'" class="SignInPopup">'+signInText+'</a></li>');
		
		//jQuery('a.SignInPopup').popup({containerCssClass:'SignInPopup'});
		
	}
	
	jQuery('.DataList').each(function() {
		
		jQuery(this).children('li:last').css({ border: 'none' });
		
	});
	
	if(jQuery('#Form_Search').val() == '') { jQuery('#Form_Search').val('Search'); }
	var searchClick = 0;
	
	jQuery('#Form_Search').click(function() { 
	
		if(searchClick === 0) {
			
			jQuery(this).val('');
			searchClick = 1;
			
		}
	
	});
	
}