<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 2/16/2016
 * Time: 6:13 PM
 */

set_include_path( "../../Models/NewUser/" .PATH_SEPARATOR .
    "../../Views/");


require_once '../../Model/NewUser/NewUser.php';
require_once '../../Model/Partial/Partial.php';
$partial = new Partial("new_user");
include ("../../Model/SQL_Classes/Connect.php");
$NewUser = new NewUser($db);
$uname = $_POST["uname"];


/* GET VARIABLES */
///type=S
//first=
//last=
//degree=
//semester=
//department=


//Submitted
if(!isset($uname)) {
    $html = $NewUser->newUser_html();

}
//Not submitted
else{

    $first =  trim($_POST["first"]);
    $last =  trim($_POST["last"]);
    $uname = trim($_POST["uname"]);
    $pass = trim($_POST["pass"]);

    //Insert new user
    if($NewUser->registerNewUser($first,$last,$uname,$pass))
        $id = $NewUser->userId;
    else{
        $error = "This username is already in use";
    }


    $name = $first . " " . $last;
    $html = $NewUser->userAdded_html($name,$id,$uname,$error);
}



require "../../View/NewUser/NewUser.php";
?>