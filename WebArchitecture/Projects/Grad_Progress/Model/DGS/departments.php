<?php
/**
 * Created by PhpStorm.
 * User: Matt Howa
 * Date: 2/5/2016
 * Time: 4:16 AM
 */

class Student
{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Semester;
    public $Year;
    public $Degree;
    public $Department;
}

class Faculty{
    public $FirstName;
    public $LastName;
    public $Id;
    public $Department;
}

class Departmnt{
    public $Name;
    public $Id;
}

class department
{
    private $db;
    private $advisors;
    private $students;

    private $depts;
    public $html;

    public function __construct($db)
    {

        $this->db = $db;

        $this->getDepartments();
        $this->dashboardHtml();




    }

    //Get students and split them by department
    public function getStudents($id)
    {
        $query = "select s.id as Id, u.FirstName, u.LastName,
                ts.Name as Semester, s.Year, td.Name as Degee
                from user as u
                inner join student as s on s.userid  = u.Id
                inner join types_semester as ts on ts.Id = s.Semesterid
                inner join types_degree as td on td.Id = s.DegreeId
                where s.id = $id";

        $stmt = $this->db->prepare($query);

        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Student");
        $returnValue = array();
        foreach ($result as $teacher) {
            array_push($returnValue, $teacher);
        }

        $this->students = $returnValue;
        return $returnValue;

    }

    public function getAllStudents()
    {
        $query = "select s.id as Id, u.FirstName, u.LastName,
                ts.Name as Semester, s.Year, td.Name as Degree, d.Name as Department
                from user as u
                inner join student as s on s.userid  = u.Id
                inner join types_semester as ts on ts.Id = s.Semesterid
                inner join types_degree as td on td.Id = s.DegreeId
                inner join advisor as a on a.Studentid = s.id
                inner join faculty as f on f.id= a.facultyid
                inner join department as d on d.id = f.departmentid
                ";

        $stmt = $this->db->prepare($query);

        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Student");
        $returnValue = array();
        foreach ($result as $teacher) {
            array_push($returnValue, $teacher);
        }

        $this->students = $returnValue;
        return $returnValue;

    }

    //Get faculty by departmentId
    public function getAdvisors($id)
    {

        $query = "select u.FirstName, u.LastName, f.Id, d.Name as Department
                 from faculty as f inner join user as u on u.Id = f.UserId
                 inner join department as d on d.Id = f.Departmentid
                 where f.id = $id";
        echo "test";


        $stmt = $this->db->prepare($query);
        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Faculty");
        $returnValue = array();
        foreach ($result as $teacher) {
            array_push($returnValue, $teacher);
        }

        $this->advisors = $returnValue;
        return $returnValue;
    }

    //Get faculty by departmentId
    public function getAllAdvisors()
    {

        $query = "select u.FirstName, u.LastName, f.Id, d.Name as Department
                 from faculty as f inner join user as u on u.Id = f.UserId
                 inner join department as d on d.Id = f.Departmentid";



        $stmt = $this->db->prepare($query);
        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Faculty");
        $returnValue = array();
        foreach ($result as $teacher) {
            array_push($returnValue, $teacher);
        }

        $this->advisors = $returnValue;
        return $returnValue;
    }

    public function getDepartments(){
        $query = "SELECT * FROM department;";

        $stmt = $this->db->prepare($query);

        $stmt->execute();

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Departmnt");
        
	$returnValue = array();
        foreach ($result as $d) {
            array_push($returnValue, $d);
        }
        $this->depts = $returnValue;
        return $returnValue;
    }


    public function byAdvisorHtml()
    {
        $this->getAllAdvisors("");

        foreach ($this->depts as $dept) {


            echo "<div >
                    <h1>$dept->Name</h1>
                  <div class='table'>
                  <div class='row' id='tableheader'>
                    <div class='col'>
                        <h4>First</h4>
                    </div>

                    <div class='col'>
                        <h4>Last</h4>
                    </div>
                  </div>";
            //Alternating row colors
            $count = 1;

            foreach ($this->advisors as $advisor) {
                if($advisor->Department != $dept->Name)
                    continue;

                if ($count == 1)
                    $count = 2;
                else $count = 1;

                echo"
                <a class='row$count' id='row' href='../../Controller/Advisor/students.php?id=$advisor->Id'>
                    <div class='col'>
                         $advisor->FirstName
                    </div>

                    <div class='col'>
                        $advisor->LastName
                    </div>
                </a>
                ";
            }


            echo "</div></div>";
        }

    }



    public function byStudentHtml()
    {
        $this->getAllStudents();

        foreach ($this->depts as $dept) {


            echo "<div >
                    <h1>$dept->Name</h1>
                  <div class='table'>
                  <div class='row' id='tableheader'>
                    <div class='col'>
                        <h4>First</h4>
                    </div>

                    <div class='col'>
                        <h4>Last</h4>
                    </div>
                    <div class='col'>
                        <h4>Degree</h4>
                    </div>
                  </div>";

            //Alternating row colors
            $count = 1;

            foreach ($this->students as $student) {



                if($student->Department != $dept->Name)
                    continue;

                if ($count == 1)
                    $count = 2;
                else $count = 1;


//<a href='../../Controller/Student/student_forms.php?id=$Student->Id'></a>
                //<a href='../../Controller/Student/student_forms.php?id=$Student->Id' id='row'>

                echo "
              <a class='row$count' id='row' href='../../Controller/Student/student_forms.php?id=$student->Id'>
                <div class='col'>
                    $student->FirstName
                </div>

                <div class='col'>
                    $student->LastName
                </div>
                <div class='col'>
                    $student->Degree
                </div>
              </a>
                ";
            }

            echo "</div></div>

            ";
        }

    }


    public function dashboardHtml(){
        $this->html ="
            <div class='jumbotron text-center'>
                <h1>DGS Overview</h1>
            </div>
            <div class='page-content'>
		  	<div class='row'>
  				<div class='col-md-6'>
  					<div class='content-box-large'>
		  				<div class='panel-heading'>
							<div class='panel-title'>Advisors</div>
						</div>
		  				<div class='panel-body'>
		  					<table class='table table-striped'>
				              <thead>
				                <tr>
				                  <th>First Name</th>
				                  <th>Last Name</th>
				                  <th>Department</th>
				                </tr>
				              </thead>
				              <tbody>
				                {$this->AdvisorsRows()}
				              </tbody>
				            </table>
		  				</div>
		  			</div>
  				</div>
  				<div class='col-md-6'>
  					<div class='content-box-large'>
		  				<div class='panel-heading'>
							<div class='panel-title'>Students</div>
						</div>
		  				<div class='panel-body'>
		  					<table class='table table-striped'>
				              <thead>
				                <tr>
				                  <th>First Name</th>
				                  <th>Last Name</th>
				                  <th>Degree</th>
				                  <th>Quick View</th>
				                </tr>
				              </thead>
				              <tbody>
				                {$this->StudentRows()}
				              </tbody>
				            </table>
		  				</div>
		  			</div>
  				</div>
  			</div>
		  </div>
		</div>
    </div>
        ";
    }



    private function AdvisorsRows(){
        $this->getAllAdvisors();
        $html="";
        foreach($this->advisors as $advisor){
            $html .="
                <tr  data-href='../../Controller/Advisor/students.php?id=$advisor->Id'>
                  <td>$advisor->FirstName</td>
                  <td>$advisor->LastName</td>
                  <td>$advisor->Department</td>
                </tr>
            ";
        }

        return $html;
    }

    private function StudentRows(){
        $this->getAllStudents();
        $html="";
        foreach($this->students as $student){
            $html .="
                <tr data-href='../../Controller/Student/student_forms.php?id=$student->Id'>
                  <td>$student->FirstName</td>
                  <td>$student->LastName</td>
                  <td>$student->Degree</td>
                  <td><a class='btn btn-primary' href='#' data-toggle='modal' onclick='quickView($student->Id)' data-target='#myModal'>
                  <span class='glyphicon glyphicon-search' aria-hidden='true'></span>
                  </a></td>
                </tr>
            ";
        }

        return $html;
    }

}