<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 2/16/2016
 * Time: 6:13 PM
 */

class Department{
    public $Name;
    public $Id;
}

class Degree{
    public $Name;
    public $Id;
}

class Semester{
    public $Name;
    public $Id;
}

class NewUser{
    public $db;
    public $userId;

    public function __construct($db)
    {
        $this->db = $db;
    }


    //Function to output newuser form
    public function newUser_html($first,$second,$error,$uname){
        $returnString = "
            <div class='container' id='reg-div'>
                <p style='color: red'>$error</p>
                <form method='post' name='register_form' class='register' onsubmit='return checkForm(this);'>
                    <input type='text' class='register-input' id='username' name='uname' value='$uname' placeholder='User Name' required>
                    <input type='password' class='register-input' id='pwd1' name='pass'  placeholder='Password' required>
                    <input type='password' class='register-input' id='pwd2'  placeholder='Confirm Password' required>
                    <input type='text' class='register-input' id='firstName' name='first' value='$first' placeholder='First Name' required>
                    <input type='text' class='register-input' id='lastName' name='last' value='$second' placeholder='Last Name' required>";





        $returnString.="
                    </select>
                    </p>
                    <p>
                        <input type='submit' value='Create Account' class='register-button'>
                    </p>
                </form>
            </div>
        ";

        return $returnString;
    }

    //Function to output new user added
    public function userAdded_html($name,$id,$uname,$error){
        $names = explode(" ", $name);
        if(isset($id)) {
            return "
            <div class='container' id='reg-div'>
                <p>
                    Thank you for Registering $name! Your uID is <b>$id</b>
                </p>
            </div>
        ";
        }
        else{

            if(!isset($error))
                $error = "*You are missing some information*";
            return "

            ".$this->newUser_html($names[0],$names[1],$error,$uname);
        }
    }

    //Get departments
    private function departments()
    {
        $query = "SELECT Name, Id FROM department";
        $stmt = $this->db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Department");
        $returnValue = array();
        foreach($result as $department)
        {
            array_push($returnValue,$department);
        }
        return $returnValue;
    }

    //Get degrees
    private function degrees()
    {
        $query = "SELECT Name, Id FROM types_degree";
        $stmt = $this->db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Degree");
        $returnValue = array();
        foreach($result as $degree)
        {
            array_push($returnValue,$degree);
        }
        return $returnValue;
    }

    //Get semesters
    private function semesters()
    {
        $query = "SELECT Name, Id FROM types_semester";
        $stmt = $this->db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll(PDO::FETCH_CLASS, "Semester");
        $returnValue = array();
        foreach($result as $semester)
        {
            array_push($returnValue,$semester);
        }
        return $returnValue;
    }

    //Make sure the user name is unique
    function checkUniqueUsername($uname){
        $query = "SELECT Count(*) from USER where UserName='$uname'";

        $stmt = $this->db->prepare($query);
        $stmt->execute();
        $result = $stmt->fetchAll();
        return $result[0][0];
    }



    public $nonUnique = false;

    // Registers a new user
    function registerNewUser($firstName, $lastName, $userName,$password)
    {

        if($firstName ==""||$lastName==""||$password==""||$userName=="")
            return false;

        if($this->checkUniqueUsername($userName)==1) {
            $this->nonUnique = true;
            return false;
        }

        try
        {
            $DBH = $this->db;
            $DBH->beginTransaction ();

            $stmt = $DBH->prepare ( "insert into user (FirstName, LastName,UserName,HashWord) values(?,?,?,?)" );
            $stmt->bindValue ( 1, $firstName );
            $stmt->bindValue ( 2, $lastName );
            //Hash the password
            $hashedPassword = $this->computeHash($password, $this->makeSalt());
            $stmt->bindValue ( 3, $userName );
            $stmt->bindValue ( 4, $hashedPassword );

            $stmt->execute ();
            $this->userId = $DBH->lastInsertId ();

            $year = date('Y');
            $stmt2 = $DBH->prepare ( "insert into student (UserId, DegreeId,SemesterId,Year) VALUES ($this->userId,1,1,$year)");
            $stmt2->execute ();
            $DBH->commit ();
            return true;
        }
        catch ( PDOException $e )
        {
            if ($e->getCode () == 23000)
            {
                //FOR TESTING
                echo $e->getMessage();

                return false;
            }
            reportDBError ( $e );
        }
    }

    // Generates random salt for blowfish
    function makeSalt () {
        $salt = strtr(base64_encode(mcrypt_create_iv(16, MCRYPT_DEV_URANDOM)), '+', '.');

        return '$2a$12$' . $salt;
    }

    // Compute a hash using blowfish using the salt.
    function computeHash ($password, $salt) {
        return crypt($password, $salt);
    }

    // Logs and reports a database error
    function reportDBError ($exception) {
        $file = fopen("application/log.txt", "a");
        fwrite($file, date(DATE_RSS));
        fwrite($file, "\n");
        fwrite($file, $exception->getMessage());
        fwrite($file, $exception->getTraceAsString());
        fwrite($file, "\n");
        fwrite($file, "\n");
        fclose($file);
        //TODO
        //require "views/error.php";
        exit();
    }
}

