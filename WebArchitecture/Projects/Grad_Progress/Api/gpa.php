<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 3/25/2016
 * Time: 8:51 PM
 */
include ("../Model/SQL_Classes/Connect.php");


function getStudentId($db){

        try {
            $query = "SELECT Year,gpa FROM student";

            $stmt = $db->prepare($query);


            $stmt->execute();

            $result = $stmt->fetchAll();

            $array= array();

            foreach($result as $r) {

                array_push($array, array($r[0],$r[1]));
            }

            echo json_encode($array);

        } catch (Exception $e) {
            //FOR TESTING
            echo $e->getMessage();
        }


}
getStudentId($db);
?>