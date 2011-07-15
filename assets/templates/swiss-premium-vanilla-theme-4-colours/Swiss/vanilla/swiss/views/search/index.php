<?php if (!defined('APPLICATION')) exit(); ?>
<div class="Tabs SearchTabs"><?php
	
	
	if ($this->SearchResults) {
?>
	<div class="SubTab"><?php printf(T(count($this->SearchResults) == 0 ? "No results for '%s'" : count($this->SearchResults)." result(s) for '%s'"), $this->SearchTerm); ?></div>
<?php } else { ?>
	<div class="SubTab">Your search did not return any results.</div>
<?php } ?>
</div>
<?php
if (is_array($this->SearchResults) && count($this->SearchResults) > 0) {
   echo $this->Pager->ToString('less');
   $ViewLocation = $this->FetchViewLocation('results');
   include($ViewLocation);
   $this->Pager->Render();
}