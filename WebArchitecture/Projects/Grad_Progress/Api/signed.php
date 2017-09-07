<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 3/26/2016
 * Time: 10:00 PM
 */


include ("../Model/SQL_Classes/Connect.php");


function getSigned($db){

    try {
       $query ="select count(s.GradProgressId) >1 as 'count', gp.StudentId
                from gradprogress as gp
                inner join  signature as s
                on s.GradProgressId = gp.Id
                group by gp.StudentId";
        $stmt = $db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();
        $signed_twice = 0;
        foreach($result as $r) {
            if($r["count"]==1)
                $signed_twice++;
        }


        $query ="select count(s.GradProgressId) <2 as 'count', gp.StudentId
                from gradprogress as gp
                inner join  signature as s
                on s.GradProgressId = gp.Id
                group by gp.StudentId";
        $stmt = $db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();
        $signed_once = 0;
        foreach($result as $r) {
            if($r["count"]==1)
                $signed_once++;
        }


        $query = "SELECT count(*) FROM  gradprogress";
        $stmt = $db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();
        $total = 0;
        foreach($result as $r) {
                $total = $r[0];
        }
        $unsigned = $total - ($signed_once + $signed_twice);




        echo json_encode(array (
            array( "name"=> "Signed by Advisor", "y"=>$signed_once ),
            array( "name"=> "Signed by Advisor & DGS", "y"=>$signed_twice ),
            array( "name"=> "Unsigned", "y"=>$unsigned )
        ));


//
//        echo json_encode($array);

    } catch (Exception $e) {
        //FOR TESTING
        echo $e->getMessage();
    }


}
getSigned($db);