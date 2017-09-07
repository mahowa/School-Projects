<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 4/3/2016
 * Time: 9:52 PM
 */
include ("../Grad_Progress/Model/SQL_Classes/Connect.php");

//submiting score
if(isset($_REQUEST["submit"])){
    $score = $_REQUEST["submit"];
    $name = $_REQUEST["name"];
    if($name =="")
        return;
    try {
        $query ="insert into tetris (score,name) values ('$score','$name')";

        $stmt = $db->prepare($query);
        $stmt->execute();

        echo "success";

    } catch (Exception $e) {
        //FOR TESTING
        //echo $e->getMessage();
    }
}
//Leaderboard Request
else{
    try {
        $query ="select * from tetris ORDER BY score DESC";

        $stmt = $db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();

        $array = array();
        foreach($result as $r) {
            array_push($array,array($r[1],$r[2]));
        }

        echo json_encode($array);


    } catch (Exception $e) {
        //FOR TESTING
        //echo $e->getMessage();
    }
}