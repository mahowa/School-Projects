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

require_once '../../Model/Student/student.php';
require_once '../../Model/Partial/Partial.php';
include ("../../Model/SQL_Classes/Connect.php");
$partial = new Partial("sform");


$id = $_SESSION["Id"];


if($_SESSION["Role"]!=0){
    $id = $_REQUEST['id'];
}

$type = $_GET['type'];

$student = new StudentProfile($db,$type,$id);

require "../../View/Student/student_forms_view.php";

?>