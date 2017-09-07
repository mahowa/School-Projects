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
        $query ="select count(AdvisorId) as 'data', concat(u.FirstName,' ', u.LastName) as 'name'
                from signature
                inner join faculty  as f on f.Id=AdvisorId
                inner join user as u on u.Id= f.UserId
                group by AdvisorId";

        $stmt = $db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();

        $array = array();
        foreach($result as $r) {
            array_push($array, array($r[1],$r[0]));
        }

        echo json_encode($array);


    } catch (Exception $e) {
        //FOR TESTING
        echo $e->getMessage();
    }


}
getSigned($db);

