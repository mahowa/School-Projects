<?php

include_once ("db_config.php");

try {

    $db = new PDO("mysql:host=$server_name;dbname=$db_name;charset=utf8", $db_user_name, $db_password);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);

}
catch(PDOException $e)
{
    echo "Connection failed: " . $e->getMessage();
}



