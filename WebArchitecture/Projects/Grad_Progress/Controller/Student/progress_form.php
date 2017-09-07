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

require_once '../../Model/Student/progress_form.php';
require_once '../../Model/Partial/Partial.php';
$partial = new Partial("dForm");

include '../../Model/SQL_Classes/Connect.php';



//Parse URL to get all input variables
$id = $_GET['id'];
$type = $_GET['type'];

$milestones = $_GET['milestone'];
$signature = $_GET['signature'];

//Grab clients ip address
if (!empty($_SERVER['HTTP_CLIENT_IP'])) {
    $ip = $_SERVER['HTTP_CLIENT_IP'];
} elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
    $ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
} else {
    $ip = $_SERVER['REMOTE_ADDR'];
}


$gid = $_GET['gid'];
$note = $_GET['note'];

//Construct model
$form = new Form( $id, $type, $db, $milestones, $signature,$ip, $gid, $note);


require "../../View/Student/progress_form_view.php";

?>