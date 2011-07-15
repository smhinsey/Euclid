<?php if (!defined('APPLICATION')) exit(); ?>
<div class="Profile">

	<div id="ProfileWrapper">
   <?php
   include($this->FetchViewLocation('user'));
   echo '<div id="ProfileTabsWrapper">';include($this->FetchViewLocation('tabs')); echo '</div>';
   include($this->FetchViewLocation($this->_TabView, $this->_TabController, $this->_TabApplication));
   ?>
</div></div>