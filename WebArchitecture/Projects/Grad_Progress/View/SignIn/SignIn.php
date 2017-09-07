<?php
echo $partial->head("Sign In");


//Try to sign in
if($uname != ""){
    if($signin->signmein($uname, $pass)){
        echo"
        <div id='myModal' class='modal fade' role='dialog'>
          <div class='modal-dialog'>

            <!-- Modal content-->
            <div class='modal-content'>
              <div class='modal-header'>
                <button type='button' class='close' data-dismiss='modal'>&times;</button>
                <h4 class='modal-title'>Success</h4>
              </div>
              <div class='modal-body'>
                <p>Welcome {$_SESSION['FirstName']} {$_SESSION['LastName']}! Redirecting Now</p>
              </div>

            </div>

          </div>
        </div><script>$('#myModal').modal('show');</script>";
    }
    else{
        echo $signin->signin_html($uname, "Username and Password do not mach our records. Please try again");
    }
}
//Ask for credentials
else
    echo $signin->signin_html($uname);





echo $partial->foot();


if($_SESSION['UserName'] != ""){

    //DGS
    if($_SESSION['Role']==2)
        header('Refresh: 3; URL=../../Controller/Main/main.php');
    //Faculty
    else if($_SESSION['Role']==1)
        header('Refresh: 3; URL=../../Controller/Advisor/students.php?id='.$_SESSION['RoleId']);
    //Student
    else if($_SESSION['Role']==0)
       header('Refresh: 3; URL=../../Controller/Student/student_forms.php?id='.$_SESSION['RoleId']);
}

