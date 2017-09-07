<?php
/**
 *
 * User: Matt
 * Date: 1/28/2016
 * Time: 6:03 PM
 */

//include_once '../SQL_Classes/MySqlQueries.php';
class StudentUnderAdvisor{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Degree;

    public function sanitize(){
        $this->FirstName = htmlspecialchars($this->FirstName);
        $this->LastName = htmlspecialchars($this->LastName);
        $this->Id = htmlspecialchars($this->Id);
        $this->Degree = htmlspecialchars($this->Degree);

    }
}
class Faculty{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Department;


    public function sanitize(){
        $this->FirstName = htmlspecialchars($this->FirstName);
        $this->LastName = htmlspecialchars($this->LastName);
        $this->Id = htmlspecialchars($this->Id);
        $this->Department = htmlspecialchars($this->Department);

    }
}

class Advisor
{
    private $Students;
    private $db;
    private $_advisor;
    public function __construct($id,$db)
    {

        $this ->db = $db;
        $this->Students =$this->getFacultyStudents($id);
	    $this->getAdvisor($id);

    }

    //Get all students for a faculty member
    function getFacultyStudents($id)
    {

        $query = "select s.id as Id, u.FirstName, u.LastName, td.Name as Degree from user as u
                    inner join student as s on s.UserId = u.id
                    inner join advisor as a on a.studentid = s.id
                    inner join faculty as f on f.id = a.facultyid
                    inner join types_degree as td on td.id = s.degreeid
                    where f.id = $id";

        $stmt = $this->db->prepare( $query );

        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "StudentUnderAdvisor");
        $returnValue = array();

        foreach($result as $student)
        {
            //Use Html Special Chars fucntion
            $student->sanitize();

            array_push($returnValue,$student);
        }



        return $returnValue;
    }

    //Get faculty by departmentId
    public function getAdvisor($id)
    {

        $query = "select u.FirstName, u.LastName, f.Id, d.Name as Department
                 from faculty as f inner join user as u on u.Id = f.UserId
                 inner join department as d on d.Id = f.Departmentid
                 where f.id = $id";

        $stmt = $this->db->prepare($query);
        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Faculty");

        $this->_advisor = $result[0];

    }


    public function html(){
        if($_SESSION["Role"]==1 ||$_SESSION["Role"]==2) {

            echo "
                <div class='container'>
                  <h2>{$this->_advisor->FirstName} {$this->_advisor->LastName} - {$this->_advisor->Department}</h2>
                  <div class='table-responsive'>
                  <table class='table'>
                    <thead>
                      <tr>
                        <th>#</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Degree</th>
                        <th>Form Status</th>
                        <th>Due Progress Form</th>
                      </tr>
                    </thead>";



            foreach ($this->Students as $Student) {
                echo "
                    <tbody>
                          <tr>
                            <td>$Student->Id</td>
                            <td>$Student->FirstName</td>
                            <td>$Student->LastName</td>
                            <td>$Student->Degree</td>
                            <td>{$this->grabDBdata($Student->Id)}</td>
                            <td>
                            <a href='../../Controller/Student/progress_form.php?id=$Student->Id&type=v' data-toggle='tooltip' data-placement='top' title='View'>
                                 <span class='glyphicon glyphicon-search' aria-hidden='true'></span>
                            </a>

                            <a href='../../Controller/Student/progress_form.php?id=$Student->Id&type=s' data-toggle='tooltip' data-placement='top' title='Sign'>
                                 <span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>
                            </a></td>
                          </tr>";
            }

            echo "</tbody>
                      </table>
                      </div>
                    </div>";
        }

        //Not signed in - Not Viewable
        else{
            echo"
                    <div class='container' id='by-department'>
                        <p>
                            You must be signed in to view this page
                        </p>
                    </div>";
        }
    }

    //Get status of form
    private function getStatus($id){
        try {
            $query = "SELECT * FROM signature where GradProgressId='$id' ";

            $stmt = $this->db->prepare($query);
            $stmt->execute();
            $result = $stmt->fetchAll();

            //Form Exists
            if (!empty($result)) {
                $returnValue = "";
                foreach($result as $r){
                    //DGS
                    if($r["SignatureTypeId"]==2)
                        $returnValue .= "Signed by DGS ";
                    if($r["SignatureTypeId"]==1)
                        $returnValue .= "Signed by Advisor ";
                }
                return $returnValue;
            }
            else {
                return "Not signed";
            }


        }
        catch ( Exception $e ){
            //FOR TESTING
            // echo $e->getMessage();
            return "Not Signed";
        }
    }
    //get student status
    private function grabDBdata($id){
        try {
            $query = "select Id as GradProgressId, Year, Note, CreatedOn
                    from gradprogress
                    where StudentId={$id} ORDER BY CreatedOn DESC LIMIT 1 ";


            $stmt = $this->db->prepare($query);
            $stmt->execute();
            $result = $stmt->fetchAll();

            //Form Exists
            if (!empty($result)) {
                return  $this->getStatus($result[0]["GradProgressId"]);
            } //Form doesnt exist then recall this function recursively
            else {
                return "No forms found";
            }
        }
        catch ( Exception $e ){
            //FOR TESTING
            // echo $e->getMessage();
        }

    }
}


