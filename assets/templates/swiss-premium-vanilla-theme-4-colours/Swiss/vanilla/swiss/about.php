<?php if (!defined('APPLICATION')) exit();
/*
Copyright 2008, 2009 Vanilla Forums Inc.
This file is part of Garden.
Garden is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
Garden is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with Garden.  If not, see <http://www.gnu.org/licenses/>.
Contact Vanilla Forums Inc. at support [at] vanillaforums [dot] com
*/

/**
 * An associative array of information about this application.
 */
$ThemeInfo['swiss'] = array(

   'Name' => 'Swiss',
   'Description' => "Premium Vanilla Theme.",
   'Version' => '1.4',
   'Author' => "designlink",
   'AuthorEmail' => 'hi@salumguilher.me',
   'AuthorUrl' => 'http://salumguilher.me',
   'Options' => array(
   
		'Description' => 'Swiss has 4 different colours to choose from. Pick yours below:',
         
		 'Styles' => array(
		 
            'Swiss Red - Default' => '%s',
            'Swiss Purple' => '%s_purple',
            'Swiss Green' => '%s_green',
            'Swiss Blue' => '%s_blue'
			
		),
		
		'Text' => array(
		
		'SLogo' => 'SLogo',
		'SAnalytics' => 'SAnalytics'
		
		)
         
   )
   
);