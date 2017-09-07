<?php

/*
*               Matthew J. Howa
*                   U0805396
*             University of Utah
*           2016 - Web Architecture

*           The controller combines the View and the model while grabs any information sent from the browser
*
*/



set_include_path( "../../Models/Student/" .PATH_SEPARATOR .
    "../../Views/");

require_once '../../Model/Advisor/advisor.php';
require_once '../../Model/Partial/Partial.php';
$partial = new Partial();
include ("../../Model/SQL_Classes/Connect.php");








$id = $_GET['id'];




$advisor = new Advisor( $id ,$db);



require "../../View/Advisor/students_view.php";

?>