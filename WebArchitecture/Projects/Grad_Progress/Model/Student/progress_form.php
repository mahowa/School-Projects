<?php


/**
 * Author: Matthew J. Howa
 * Date: Spring 2016
 *
 * This is the Model for our Graduate Due Progress Form
 *
 */

class _Form{
    public $ComitteeName;
    public $Description;
    public $StudentFirst;
    public $StudentLast;
    public $StudentId;
    public $Degree;
    public $Year;
    public $Semester;
    public $Note;
    public $Date;
}
class Milestone{
    public $Id;
    public $Semester;
    public $Year;
    public $Good;
    public $Adequet;
}
class Milestone_A{
    public $Id;
    public $Name;
    public $Good;
    public $Adequet;
}
class Milestone_B{
    public $Id;
    public $Name;
    public $Good;
    public $Adequet;
    public $Semester;
    public $Year;
}
class GradProgress{
    public $Id;
    public $StudentId;
    public $Year;
    public $Note;
    public $CreatedOn;
}


class Form
{
    public $progress = array("0","0","0","0","0","0","0","0","0","0");
    public $_form;
    public $milestones;
    public $all_milestones;
    public $thisMilestones= array();
    public $rowCount = 1;
    private $type;
    private $id;
    private $sig;
    private $ip;
    public $gp;
    private $db;
    private $gid;
    private $note;

    public function __construct($id, $type, $db, $toDB, $sig,$ip, $gid, $note)
    {

        $this->ip = $ip;
        $this->db = $db;
        $this->type = $type;
        $this->id = $id;
        $this->sig = $sig;
        $this->gid = $gid;
        $this->note = $note;
        $this->getCurrentGradProgress();

        if(isset($gid)){
            $this->note = $note;
            $this->updateNote();
            $this->gp[0]->Note = $note;
        }


        if(!isset($toDB) && !isset($sig)) {

            $this->getForm($id);
            $this->getMilestone($id);
            $this->getAllMilestones();
            $this->setMilestones();
        }
        else if(isset($sig)){


            $this->signForm();
        }
        else {
            if($this->saveToDB($toDB));
                //TODO let the user know

        }

    }


    //Save milestone to database
    private function saveToDB($toDB){
        foreach($toDB as $milestone){
            try
            {
                $DBH = $this->db;
                $DBH->beginTransaction ();

                $stmt = $DBH->prepare ( "insert into student_milestones (StudentId, SemesterId,Year,MilestoneTypeId) values(?,?,?,?)" );
                $stmt->bindValue ( 1, $this->id );

                //TODO get the real semester Id
                $stmt->bindValue ( 2, 1);

                $date = DateTime::createFromFormat("Y-m-d", "2068-06-15");
                $stmt->bindValue ( 3, $date->format("Y"));

                $stmt->bindValue ( 4, $milestone);
                $stmt->execute ();
                $this->userId = $DBH->lastInsertId ();

                $DBH->commit ();
                return true;
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


    //Update Note
    private function updateNote(){
        try
        {
            $DBH = $this->db;
            $DBH->beginTransaction ();

            $stmt = $DBH->prepare ( "update gradprogress set Note='$this->note' where id='$this->gid'" );

            $stmt->execute();

            $DBH->commit ();
            return true;
        }
        catch ( PDOException $e )
        {
            if ($e->getCode () == 23000)
            {
                //FOR TESTING
               // echo $e->getMessage();
                return false;
            }
            reportDBError ( $e );
        }


    }


    //Sign the form
    private function signForm(){
        try
        {
            $DBH = $this->db;
            $DBH->beginTransaction ();


            $stmt = $DBH->prepare ( "insert into signature (Signature, IpAddress,SignatureTypeId,GradProgressId, AdvisorId) values(?,?,?,?,?)" );
            $stmt->bindValue ( 1, $this->sig);
            $stmt->bindValue ( 2, $this->ip);
            $stmt->bindValue ( 3, 1);
            $stmt->bindValue ( 4, $this->gid);
            $stmt->bindValue ( 5, $this->id); //TODO Get Advisor Id
            $stmt->execute ();

            $this->userId = $DBH->lastInsertId ();




            $DBH->commit ();
            header('Location: ../Student/progress_form.php?id='.$this->id);
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

    //Get a user form
    private function getForm($id)
    {
        $query = "SELECT u.FirstName as StudentFirst, u.LastName as StudentLast, s.Id as StudentId,d.Name as Degree,
                    s.Year,ts.Name as Semester,gp.Note as Note, Max(gp.CreatedOn) as Date
                    From user as u
                    inner join student as s on s.Userid = u.Id
                    inner join types_degree as d on d.id=s.degreeid
                    inner join types_semester ts on ts.id = s.SemesterId
                    inner join gradprogress as gp on gp.StudentId = s.Id
                    where s.id=$id
                    group by u.id";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "_Form");
        $this->_form = $result[0];

        return $result[0];
    }

    //Get a milestones
    public function getMilestone($id)
    {
        $query =    "SELECT tm.Id, sm.Year, ts.Name, tm.Good,tm.Adequet as Semester
                    from student_milestones as sm
                    inner join types_semester as ts on ts.id = sm.Semesterid
                    inner join types_milestone as tm on tm.Id = sm.MilestoneTypeId
                    where sm.studentid = $id";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Milestone");
        $returnValue = array();
        foreach($result as $milestone)
        {
            array_push($returnValue,$milestone);
        }

        $this->milestones = $returnValue;

        return $returnValue;
    }

    //Get a milestones
    public function getCurrentGradProgress()
    {
        $query =    "SELECT * FROM gradprogress where StudentId = $this->id ORDER BY CreatedOn DESC LIMIT 1";

        $statement = $this->db->prepare( $query );
        $statement->setFetchMode(PDO::FETCH_CLASS, 'GradProgress');
        $statement->execute();

        $this->gp  = $statement->fetchAll();



    }

    //Format html for output
    public function html(){
        //Either the student whose the form belongs to, DGS or Advisor

        if(isset($_SESSION["UserName"]) && ($this->id== $_SESSION['RoleId'] || $_SESSION['Role']>0)) {
            $html = "
        <div class='jumbotron text-center'>
            <p><label> Date: {$this->_form->Date} </label></p>
            <p><label for='sName'><strong>Student Name: </strong>{$this->_form->StudentFirst} {$this->_form->StudentLast}  </label>
                <label for='sID'><strong>Student ID#:</strong> {$this->_form->StudentId}</label></p>
            <p ><label><strong>Degree:</strong> {$this->_form->Degree} </label></p>
            <p><label for='Semester'><strong>Semester Admitted:</strong> {$this->_form->Semester}</label></p>
            <p><label for='num-Semester'><strong> # of Semesters in the program:</strong> X</label></p>
            <p><label for='advisor'><strong> Advisor:</strong></label></p>
            <p><label for='committee'><strong> Committee:</strong> {$this->_form->ComitteeName}</label></p>
            <p><label for='committee'><strong> Description: </strong>{$this->_form->Description}</label></p>
        </div>
        <div class='container'>
          <div class='table'>
          <table class='table table-striped'>
            <thead>
              <tr>
                <th>Milestone</th>
                <th>Good</th>
                <th>Acceptable</th>
                <th>Pass</th>
                <th>Semester</th>
              </tr>
            </thead>
                <tbody id='milestone'>" .
                $this->formatMilestonesHtml()
                . "
                </tbody>
              </table>
              </div>
            </div>";

            //HTML FOR NEW AND EDITING FROMS
            if ($this->type == "n" || $this->type == "e") {
                $html .= "
            <div class='container'>
                <p>
                    Describe the progress made during the past year?
                </p>
                <p>
                    <div class='form-group'>
                      <label for='comment'>Comment:</label>
                      <textarea class='form-control' rows='5' id='comment' >".$this->gp[0]->Note."</textarea>
                    </div>
                </p>
            </div>
        ";
                $html .= "

                <div class='container form-group '>
                    <form id='milestone-form'>
                        <input type='hidden' name='id' id='StudentId' value='$this->id'/>
                        <input type='hidden' name='gid' id='gId' value='" . $this->gp[0]->Id . "'/>
                        <label for='milestone-select'>Milestone</label>
                        <select id='milestone-select' class='form-control'>
                            <option >Select Milestone</option>
                            " . $this->milestonOptions() . "
                        </select>
                        <br/>
                        <div class='text-center'>
                        <input type='button' class='btn btn-danger' onclick='addMilestone()' value='Add'/>
                        <p>
                        <br/>
                            <input class='btn' type='submit' value='Save'/>
                        </p>
                        </div>
                    </form>
                </div>
                ";
            } //HTML FOR Signing
            else if ($this->type == "s") {
                $html .= "
                    <div class='container'>
                        <p>
                            Describe the progress made during the past year?
                        </p>
                        <p>
                            <div class='form-group'>
                              <label for='comment'>Comment:</label>
                              <textarea class='form-control' rows='5' id='comment' readonly>".$this->gp[0]->Note."</textarea>
                            </div>

                        </p>
                    </div>
                ";
                $html .= "

                <div style='margin: auto; align-content: center;margin-top: 25px;text-align: center'>
                    <form id='milestone-form'>
                        <input type='hidden' name='id' id='StudentId' value='" . $this->id . "'/>
                        <input type='hidden' name='gid'  value='" . $this->gp[0]->Id . "'/>
                        <p>
                            <label for='signature'><input name='signature' id='signature' type='text' placeholder='Full Name'/></label>
                        </p>
                        <input type='submit' value='sign'/>
                    </form>
                </div>";

            } //HTML for everything else
            else {
                $html .= "
            <div class='container'>
                <p>
                    Describe the progress made during the past year?
                </p>
                <p>
                    <div class='form-group'>
                      <label for='comment'>Comment:</label>
                      <textarea class='form-control' rows='5' id='comment' readonly>".$this->gp[0]->Note."</textarea>
                    </div>
                </p>
            </div>
        ";
            }
        }
        //Does not belong to student - Not Viewable
        else if(isset($_SESSION['UserName'])) {
            $html = "
                    <div class='container' id='by-department'>
                        <p>
                            You do not have permission to see this page
                        </p>
                    </div>";


        }
        //Not signed in - Not Viewable
        else{
            $html = "
                    <div class='container' id='by-department'>
                        <p>
                            You must be signed in to view this page
                        </p>
                    </div>";

        }

        return $html;
    }


    //Combine the too classes for ease of use
    private function rowMilestone($milestone, $milestone_a){

        $m = new Milestone_B();

        $m->Id = $milestone->ID;

        $m->Name = $milestone_a->Name;
        $m->Good = $milestone_a->Good;
        $m->Adequet = $milestone_a->Adequet;
        $m->Semester = $milestone->Semester;
        $m->Year = $milestone->Year;

        return $m;
    }

    //Format milestone html
    public function formatMilestonesHtml(){
        $html="";
        foreach($this->thisMilestones as $milestone) {
            $html .= "
                <tr>
                    <td>$milestone->Name</td>
                    <td> <img data-toggle='popover' title='Good' data-content='$milestone->Good' src='../../../../img/green-check.png' height='25' width='25' ></td>
                    <td> <img data-toggle='popover' title='Adequet' data-content='$milestone->Adequet' src='../../../../img/yellow-check.png' height='25' width='25'></td>
                    <td> <img data-toggle='popover' title='Pass' data-content='Completed, Time to get Going' src='../../../../img/red-check.png' height='25' width='25'></td>
                    <td>$milestone->Semester</td>
                   </tr>";
        }

        return $html;
    }


    //Format milestone option html
    public function milestonOptions(){
        $options= "";

        foreach($this->all_milestones as $am) {
            $options .= "<option value='$am->Id'>$am->Name</option>";
        }
        return $options;

    }


    //get the list of milestones
    private function  getAllMilestones(){

        $query =    "select * from types_milestone";

        $stmt = $this->db->prepare( $query );
        $stmt->execute(  );

        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Milestone_A");

        $returnValue = array();
        foreach($result as $milestone)
        {
            array_push($returnValue,$milestone);
        }

        $this->all_milestones = $returnValue;

        return $returnValue;
    }

    //Get the milestones that the user currently has
    public function setMilestones(){

        //Unset all the milestones that already exist
        foreach($this->milestones as $milestone){
            foreach($this->all_milestones as $aM){
                if($aM->Id == $milestone->Id) {
                    array_push($this->thisMilestones, $this->rowMilestone($milestone, $aM));
                    unset($this->all_milestones[(int)$aM->Id]);
                    break;
                }
            }
        }
    }

    public  function description(){
        echo "

                    <div class='form-group'>
                      <label for='comment'>Progress made during the past year:</label>
                      <textarea class='form-control' rows='5' id='comment' readonly>".$this->gp[0]->Note."</textarea>
                    </div>

            ";
    }
}

