<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en-ca">

    <head>
    
		{asset name='Head'}
        <!-- /imports head/ -->
        
        {literal}
        
        <script type="text/javascript">
		
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
		
			jQuery(document).ready(function() {
				
				discussionButton();
				
			});
		
		</script>
        
        {/literal}
        
    </head>
    <!-- /head ends/ -->

	<body id="{$BodyID}" class="{$BodyClass}">

		<div id="wrapper">
        
        	<div id="Head">
            
            	<div id="logo" class="sidebar-block">
                
                		{capture assign="LogoDefault"}|def|/themes/swiss/design/images/logo.gif{/capture}
                        {capture assign="Logo"}{text code="SLogo"}{/capture}
                        {if $Logo}{capture assign="LogoFinal"}{$Logo}{/capture}{else}{capture assign="LogoFinal"}{$LogoDefault}{/capture}{/if}
                    
                	<a href="{link path="/"}"><img src="{$LogoFinal}" alt="" /></a>
                
                </div>
                <!-- /#logo/ -->
                
                <div id="menu" class="sidebar-block">
                
                	<ul>
                    
                    	<li><a href="{link path="discussions"}">Discussions</a><a href="{link path="discussions/feed.rss"}" id="discussions-rss"></a></li>
                        
                        {if CheckPermission('Garden.Settings.Manage')}
                       <li><a href="{link path="dashboard/settings"}">Dashboard</a></li>
                    {/if}
                        
                        <li><a href="{link path="activity"}">Activity</a></li>
                        
                        {if $User.SignedIn}
                        <!-- If the user is signed in -->
                        
                            <li>
                            	<a href="{link path="messages/inbox"}">Inbox
                            	{if $User.CountUnreadConversations} <span>{$User.CountUnreadConversations}</span>{/if}
                                </a>
                                
                            </li>
                            
                            <li>
                            
                            	<a href="{link path="profile"}">{$User.Name}
                            	{if $User.CountNotifications} <span>{$User.CountNotifications}</span>{/if}
                                </a>
                                
                            </li>
                            
                    	{/if}
                        
                        {custom_menu}
                        
                    	<li>{link path="signinout"}</li>
                    
                    </ul>
                    <!-- /#menu ul/ -->
                
                </div>
                <!-- /#menu -->
                
                <div id="search" class="sidebar-block">
                
                	{searchbox}
                
                </div>
                <!-- /#search/ -->
                
                <div id="categories" class="sidebar-block">
                
                	{asset name="Panel"}
                
                </div>
                <!-- /.sidebar-block/ -->
            
            </div>
            <!-- /#head/ -->
      
          <div id="Body">
          
             <div id="Content">{asset name='Content'}
             
             <div id="Copyright">Powered by Vanilla.</div>
             
             </div>
             
          </div>
          
      <div id="Foot">
			{asset name='Foot'}
		</div>
        
			</div>
            <!-- /#wrapper/ -->
            
            {capture assign="SAnalytics"}{text code="SAnalytics"}{/capture}
            {if $SAnalytics}{$SAnalytics}{/if}
        
		</body>
        <!-- /body ends/ -->

</html>
