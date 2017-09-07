<?php
/**
 * Created by PhpStorm.
 * User: Matt Dont Break Me
 * Date: 2/2/2016
 * Time: 11:45 PM
 *
 *
 *
 * This class is used for testing purposes only and also as a place to grab and reuse the code throughout my models
 *
 *                      *This is a template and testing class*
 */

//require_once ("Connect.php");

//
//class TableRows extends RecursiveIteratorIterator {
//    function __construct($it) {
//        parent::__construct($it, self::LEAVES_ONLY);
//    }
//
//    function current() {
//        return "<td style='width:150px;border:1px solid black;'>" . parent::current(). "</td>";
//    }
//
//    function beginChildren() {
//        echo "<tr>";
//    }
//
//    function endChildren() {
//        echo "</tr>" . "\n";
//    }
//}
//
//


class Student
{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Semester;
    public $Year;
    public $Degree;

}
class Faculty{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Department;
}

class StudentUnderAdvisor{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Degree;
}

class Form{
    public $ComitteeName;
    public $Description;
    public $StudentFirst;
    public $StudentLast;
    public $StudentId;
    public $Degree;
    public $Year;
    public $Semester;
    public $Progress;
}

class Milestone{
    public $Id;
    public $Semester;
    public $Year;
}

class Queries{
    private $db;
    function __construct($database) {

        $this->db = $database;

    }

    //Get a user from the db using their student id
    function getStudent($id){
        $query = "select s.id as Id, u.FirstName, u.LastName,
                ts.Name as Semester, s.Year, td.Name as Degee
                from user as u
                inner join student as s on s.userid  = u.Id
                inner join types_semester as ts on ts.Id = s.Semesterid
                inner join types_degree as td on td.Id = s.DegreeId
                where s.id = $id";

        $stmt = $this->db->prepare( $query );

        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Student");
        $returnValue = array();
        foreach($result as $teacher)
        {
            array_push($returnValue,$teacher);
        }

        return $returnValue;
    }

    //Get a faculty member from the db
    function getFaculty($id)
    {
        $query = "select u.FirstName, u.LastName, f.Id, d.Name as Department
                 from faculty as f inner join user as u on u.Id = f.UserId
                 inner join department as d on d.Id = f.Departmentid
                 where f.id = $id";

        $stmt = $this->db->prepare( $query );
        $stmt->setFetchMode(PDO::FETCH_CLASS, 'Faculty');
        $stmt->execute(  );

        $user    = $stmt->fetchAll();
        return $user;
    }


    //Get all students for a faculty member
    function getFacultyStudents($id)
    {
        $query = "select s.id as Id, u.FirstName, u.LastName, td.Name as Degree from user as u
                    inner join student as s on s.UserId = u.id
                    inner join Advisor as a on a.studentid = s.id
                    inner join Faculty as f on f.id = a.facultyid
                    inner join types_degree as td on td.id = s.degreeid
                    where f.id = $id";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );



        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "StudentUnderAdvisor");
        $returnValue = array();

        foreach($result as $student)
        {
            array_push($returnValue,$student);
        }



        return $returnValue;
    }



    //Get a faculty member from the db
    function getAllFaculty()
    {
        $query = "select u.FirstName, u.LastName, f.Id, d.Name as Department
                 from faculty as f inner join user as u on u.Id = f.UserId
                 inner join department as d on d.Id = f.Departmentid";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Faculty");
        $returnValue = array();
        foreach($result as $teacher)
        {
            array_push($returnValue,$teacher);
        }

        return $returnValue;
    }

    //Get a user form
    function getForm($id)
    {
        $query = "SELECT c.Name as ComitteeName, c.Description, u.FirstName as StudentFirst,
                    u.LastName as StudentLast, s.Id as StudentId,d.Name as Degree, s.Year,ts.Name as Semester, MAX(gp.Note) as Progress
                    FROM commitee as c inner join Student as s on s.id = c.StudentId
                    inner join user as u on u.id = c.studentId
                    inner join types_degree as d on d.id=s.degreeid
                    inner join types_semester ts on ts.id = s.SemesterId
                    inner join gradprogress as gp on gp.StudentId = s.Id
                    where s.id=$id
                    group by u.id";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Form");

        return $result[0];
    }


    //Get a milestones
    function getMilestone($id)
    {
        $query =    "SELECT sm.ID, sm.Year, ts.Name as Semester
                    from student_milestones as sm
                    inner join types_semester as ts on ts.id = sm.Semesterid
                    where sm.studentid = 1";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Milestone");
        $returnValue = array();
        foreach($result as $milestone)
        {
            array_push($returnValue,$milestone);
        }

        return $returnValue;
    }

    //Get all students by department


}



