<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 5/3/2016
 * Time: 8:58 PM
 */

$server_name  = 'localhost';
$db_user_name = 'root'; //howa
$db_password  = 'root';
//$db_password  = '013645325';
$db_name      = 'dh_svg';

try {

    $db = new PDO("mysql:host=$server_name;dbname=$db_name;charset=utf8", $db_user_name, $db_password);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);

}
catch(PDOException $e)
{
    echo "Connection failed: " . $e->getMessage();
}

if(!isset($_REQUEST["title"]))
    exit();



$title = $_REQUEST["title"];


try {
    $DBH = $db;
    $query = "SELECT  * FROM templates where PRODUCT_TITLE = '$title' LIMIT 1";
    $stmt = $DBH->prepare($query);
    $stmt->execute();

    while ($row = $stmt->fetch()) {
        echo json_encode($row);
    }
} catch (PDOException $e) {

}
