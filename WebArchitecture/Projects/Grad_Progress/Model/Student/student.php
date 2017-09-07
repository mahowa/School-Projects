<?php
/**
 * Created by PhpStorm.
 * User: Matt Howa
 * Date: 1/28/2016
 * Time: 6:02 PM
 */

class StudentProfile
{
    public $html;
    private $status;
    private $lastEdited;
    private $db;
    private $gradProgressId;
    private $type;
    private $Year;
    private $Note;
    private $CreatedOn;
    private $StudentId;
    private $id;

    public function __construct($db,$type,$ID)
    {
        $this->db = $db;
        $this->type = $type;
        $this ->id = $ID;

        $this->getStudentId();

        if($type == "n"){
            $this->addNewForm();
        }
        else{
            $this->grabDBdata();
        }
        $this->sethtml();

    }
    //setter for the html
    public function sethtml(){

        $this->html = "
        <div class='container'>
          <h2>Student Profile</h2>
          <div class='table-responsive'>
          <table class='table'>
            <thead>
              <tr>
                <th>Last Updated</th>
                <th>Status</th>
                <th>Options</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>$this->CreatedOn</td>
                <td>$this->status</td>
                <td>";

        if($_SESSION["Role"] == 0) {
            $this->html .= "
            <a href = '../../Controller/Student/progress_form.php?id=$this->StudentId&type=e' data-toggle='tooltip' data-placement='top' title='Edit'>
                <span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>
            </a >
            <a href='../../Controller/Student/student_forms.php?id=$this->StudentId&type=n' data-toggle='tooltip' data-placement='top' title='New'>
                <span class='glyphicon glyphicon-plus' aria-hidden='true'></span>
            </a>";
        }
        else{
            $this->html .= "<a href = '../../Controller/Student/progress_form.php?id=$this->StudentId&type=s' data-toggle='tooltip' data-placement='top' title='Sign'>
                            <span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>
                        </a >";
        }
        $this->html .="</td>

              </tr>
            </tbody>
          </table>
          </div>
        </div>";

    }


    //get student Id
    public function getStudentId(){

        if($_SESSION["Role"]!=0){

            $this->StudentId = $this->id;
        }
        else {
            try {
                $query = "select Id from student
                    where UserId=" . $this->id;

                $stmt = $this->db->prepare($query);
                $stmt->execute();

                $result = $stmt->fetch();
                $this->StudentId = $result["Id"];
            } catch (Exception $e) {
                //FOR TESTING
                //echo $e->getMessage();
            }
        }

    }

    //get student status
    public function grabDBdata(){
        try {
            $query = "select Id as GradProgressId, Year, Note, CreatedOn
                    from gradprogress
                    where StudentId={$this->StudentId} ORDER BY CreatedOn DESC LIMIT 1 ";


            $stmt = $this->db->prepare($query);
            $stmt->execute();
            $result = $stmt->fetchAll();

            //Form Exists
            if (!empty($result)) {
                $this->gradProgressId = $result[0]["GradProgressId"];
                $this->Year = $result[0]["Year"];
                $this->Note = $result[0]["Note"];
                $this->CreatedOn = $result[0]["CreatedOn"];

                $this->getStatus();
            } //Form doesnt exist then recall this function recursively
            else {
                $this->addNewForm();
                $this->grabDBdata();
            }


        }
        catch ( Exception $e ){
            //FOR TESTING
           // echo $e->getMessage();
        }

    }

    private function getStatus(){
        try {
            $query = "SELECT * FROM signature where GradProgressId='$this->gradProgressId' ";


            $stmt = $this->db->prepare($query);
            $stmt->execute();
            $result = $stmt->fetchAll();

            //Form Exists
            if (!empty($result)) {
                foreach($result as $r){
                    //DGS
                    if($r["SignatureTypeId"]==2)
                        $this->status .= "Signed by DGS ";
                    if($r["SignatureTypeId"]==1)
                        $this->status .= "Signed by Advisor ";
                }
            } //Form doesnt exist then recall this function recursively
            else {
                $this->status="Not signed";
            }


        }
        catch ( Exception $e ){
            //FOR TESTING
            // echo $e->getMessage();
        }
    }


    public function addNewForm(){
        try
        {
            $DBH = $this->db;
            $DBH->beginTransaction ();
            $createdOn= date('Y-m-d H:i:s');
            $date =date('Y');

            $query = "insert into gradprogress (StudentId, Year,CreatedOn)
                                     values('$this->StudentId','$date','$createdOn')";
            $stmt = $DBH->prepare ($query);

            $stmt->execute ();
            $this->gradProgressId = $DBH->lastInsertId ();

            $DBH->commit ();


            header('Location: progress_form.php?id='.$_SESSION['RoleId'].'&type=v');
        }
        catch ( PDOException $e )
        {
            if ($e->getCode () == 23000)
            {
                //FOR TESTING
                //echo $e->getMessage();

                return false;
            }
            reportDBError ( $e );
        }

    }
}


