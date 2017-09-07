<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 5/3/2016
 * Time: 7:56 PM
 */
if(!isset($_REQUEST["directory"]))
    exit();

$base = $_REQUEST["directory"];
$dir = "../COLOR_PRODUCT_IMAGE/".$base;
$files = scandir($dir);

unset($files[0]);
unset($files[1]);

$values = array();
foreach($files as $file){
    array_push($values, $file);
}

echo json_encode($values);
