<?php



class SignIn
{

    private $db;

    public function __construct($db)
    {
        $this->db = $db;
    }

    //Function to output newuser form
    public function signin_html($uname,$error)
    {
        $returnString = "
            <div class='col-sm-4'></div>
            <div class='col-sm-4'>
                <form class='form-signin' method='post'>
                <h2 class='form-signin-heading'>Please sign in</h2>";

        if(isset($error))
            $returnString .="<div class='alert alert-danger' >$error</div>";

        $returnString.="
                <label for='inputEmail' class='sr-only'>User Name</label>
                <input type='text' id='username' class='form-control' name='uname' placeholder='User Name' value='$uname' required autofocus>
                <br/>
                <label for='inputPassword' class='sr-only'>Password</label>
                <input type='password' id='inputPassword' class='form-control' name='pass' placeholder='Password' required>
                <div class='checkbox'>
                </div>
                <button class='btn btn-lg btn-primary btn-block' type='submit'>Sign in</button>
              </form>
                <br/>
              <div class='well well-large text-center'>
               FOR EASY GRADING.

                    <br>
                    <b>DGS:</b><br/>
                    Username: director<br/>
                    Password: Password1<br/>
                    <br/>

                    <b>Faculty:</b><br/>
                    Username: faculty<br/>
                    Password: Password1<br/>
                    <br/>

                    <b>Student:</b><br/>
                    Username: student<br/>
                    Password: Password1<br/>
                    <br/>
              </div>

            </div> <!-- /container -->
            <div class='col-sm-4'></div>

        ";

        return $returnString;
    }

    //Function to output newuser form
    public function signedin_html()
    {

        $returnString = "
            <div class='container' >
                <p>
                    Welcome ".$_SESSION['FirstName']." ". $_SESSION['LastName']."! Redirecting...
                </p>
            </div>
        ";

        return $returnString;
    }

    public function signmein($uname, $pass)
    {

        if($uname == "" || $pass=="" ){
            return $this->signin_html($uname,"Please enter missing information");
        }

        try {
            $DBH = $this->db;
            $query = "select IsAdmin, FirstName,LastName,Id, HashWord
                      from user as u
                      where UserName=?";
            $stmt = $DBH->prepare($query);



            $stmt->bindValue(1, $uname);
            $stmt->execute();

            while ($row = $stmt->fetch()) {
                $hashedPassword = $row['HashWord'];

                if($this->computeHash($pass,$hashedPassword)==$hashedPassword){
                    $_SESSION["UserName"] = $uname;
                    $_SESSION["FirstName"] = $row['FirstName'];
                    $_SESSION["LastName"] = $row['LastName'];
                    $_SESSION["Role"] = $row['IsAdmin'];
                    $_SESSION["Id"] = $row['Id'];


                    $this->getRoleId($row['Id']);
                    return true;
                }
                else{
                    return false;
                }
            }
        } catch (PDOException $e) {
            reportDBError($e);
        }
    }


    private function getRoleId($id){
        try {
            $DBH = $this->db;
            $query = "select id as RoleId
                      from student
                      where UserId=$id";

            $stmt = $DBH->prepare($query);

            $stmt->execute();

            $row = $stmt->fetch();



            if (!empty($row)) {
                $_SESSION["RoleId"] = $row['RoleId'];
               return true;
            } else {
                $query = "select id as RoleId
                      from faculty
                      where UserId=$id";

                $stmt = $DBH->prepare($query);
                $stmt->execute();
                $row = $stmt->fetch();
                $_SESSION["RoleId"] = $row['RoleId'];

                return true;
            }

        }catch (PDOException $e) {

        }
        return false;
    }


    // Generates random salt for blowfish
    function makeSalt()
    {

        $salt = strtr(base64_encode(mcrypt_create_iv(16, MCRYPT_DEV_URANDOM)), '+', '.');
        return '$2a$12$' . $salt;
    }

    // Compute a hash using blowfish using the salt.
    function computeHash($password, $salt)
    {
        return crypt($password, $salt);
    }


    function clear(){
        unset($_SESSION["UserName"]);
        unset($_SESSION["FirstName"]);
        unset($_SESSION["LastName"]);
        unset($_SESSION["Id"]);
        unset($_SESSION["Role"]);
        unset($_SESSION["RoleId"]);

       // header('Location: ../../Controller/SignIn/SignIn.php');
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
       // require "views/error.php";
        exit();
    }
}