<?php

/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 2/16/2016
 * Time: 6:13 PM
 */

set_include_path( "../../Models/SignIn.php/" .PATH_SEPARATOR .
    "../../Views/");

require_once '../../Model/SignIn/SignIn.php';
include ("../../Model/SQL_Classes/Connect.php");

require_once '../../Model/Partial/Partial.php';
$partial = new Partial("sign_in");


$signin = new SignIn($db);


if(isset($_GET["logout"])){
    $signin->clear();
}

if(isset($_POST["uname"])) {
    $uname = $_POST["uname"];
    $pass = $_POST["pass"];
}


require "../../View/SignIn/SignIn.php";


?>