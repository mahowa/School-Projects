<?php

/*
*               Matthew J. Howa
*                   U0805396
*             University of Utah
*           2016 - Web Architecture

*           The controller combines the View and the model while grabs any information sent from the browser
*
*/


set_include_path( "../../Model/DGS/" .PATH_SEPARATOR .
    "../../Views/");

require_once '../../Model/DGS/departments.php';
require_once '../../Model/Partial/Partial.php';
$partial = new Partial("dForm");
include ("../../Model/SQL_Classes/Connect.php");

$id = $_GET["id"];


$dept = new department($db);


require "../../View/DGS/overview_view.php";

?>