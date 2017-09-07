<?php

/*
*               Matthew J. Howa
*                   U0805396
*             University of Utah
*           2016 - Web Architecture

*           The controller combines the View and the model while grabs any information sent from the browser
*
*/


set_include_path( "../../Model/Main/" .PATH_SEPARATOR .
    "../../Views/");

require_once '../../Model/Main/main.php';
require_once '../../Model/Partial/Partial.php';
$partial = new Partial();
$main = new main();

require "../../View/Main/main.php";

?>