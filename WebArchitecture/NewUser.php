<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html lang="en">
<head>

    <!-- Last Updated Spring 2016 -->
    <!-- Matthew J. Howa -->
    <!-- University of Utah -->

    <!-- This is my class homepage  -->


    <title> Home </title>

    <!-- Meta Information about Page -->
    <meta charset="utf-8"/>
    <meta name="AUTHOR"      content="Matthew J. Howa"/>
    <meta name="keywords"    content="HTML, Template"/>
    <meta name="description" content="Landing page for CS4140- Web Architecture"/>

    <!-- ALL CSS FILES -->
    <link rel="stylesheet" href="styles/nav-bar.css" type="text/css"/>
    <link rel="stylesheet" href="styles/home-page.css" type="text/css"/>
    <link rel="stylesheet" href="styles/Register.css" type="text/css"/>
    <link rel="stylesheet" href="styles/general.css" type="text/css"/>
    <!-- ALL SCRIPT FILES -->
<!--    <script src="js/ajax.js"> </script>-->
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.0.min.js"></script>
    <script type="application/javascript">
        $(document).ready(function(){
            switchToStudent();
        });
        //Show student options
        function switchToStudent () {
            var semester = document.getElementById("semester");
            semester.style.display = 'inline';
            var degree = document.getElementById("degree");
            degree.style.display = 'inline';
            var department = document.getElementById("department");
            department.style.display = 'none';
        }
        //Show faculty options
        function switchToFaculty () {
            var semester = document.getElementById("semester");
            semester.style.display = 'none';
            var degree = document.getElementById("degree");
            degree.style.display = 'none';
            var department = document.getElementById("department");
            department.style.display = 'inline';
        }

    </script>

</head>

<body>

<div class="nav-bar">
    <ul>
        <li><a class="active"  href="index.php">Home</a></li>
        <li><a href="ClassExamples/index.html">Class Examples</a></li>
        <li><a href="Projects/index.html">Projects</a></li>
        <li><a href="README.php">ReadMe</a></li>
        <li><a href="schema.php">Schema</a></li>
        <li><a href="V1/">V1</a></li>
        <li><a href="V2/">V2</a></li>
        <li><a href="V3/">V3</a></li>
        <a href="https://github.com/uofu-cs4540/805396"><img class="gitLink" src="img/Octocat.png" alt="GitHub" width="30" height="30"/></a>
    </ul>
</div>

<h1> Register </h1>

<div class="container" id="reg-div">
    <form name="register_form" class="register">
        <div class="register-switch">
<!--            <input type="radio" name="type" value="F" id="type_f" class="register-switch-input2" checked>-->
<!--            <label for="type_f" class="register-switch-label2">Faculty</label>-->
<!--            <input type="radio" name="type" value="M" id="type_f" class="register-switch-input2">-->
<!--            <label for="type_s" class="register-switch-label2">Student</label>-->
            <input type="radio" name="type" value="S" id="sex_f" class="register-switch-input" onchange="switchToStudent();" checked>
            <label for="sex_f" class="register-switch-label">Student</label>
            <input type="radio" name="type" value="F" id="sex_m" class="register-switch-input"  onchange="switchToFaculty();">
            <label for="sex_m" class="register-switch-label">Faculty</label>
        </div>

        <input type="text" class="register-input" name="first" placeholder="First Name">
        <input type="text" class="register-input" name="last" placeholder="Last Name">

        <p>
        <select id="degree" name="degree">
            <option value="">Please select a degree</option>
            <option value="CS">Computer Science</option>
            <option value="CE">Computer Engineering</option>
        </select>
        </p>
        <p>
<!--        TODO: create some php code to dynamically generate semesters    -->
        <select id="semester" name="semester">
            <option value="">Please select a semester</option>
            <option value="SU16">SU16 - Summer 2016 </option>
            <option value="FA16">FA16 - Fall 2016</option>
            <option value="SP17">SP17 - Spring 2016</option>
        </select>

        <select id="department" name="department">
            <option value="">Please select a department</option>
            <option value="CS">Computer Science </option>
            <option value="CE">Computer Engineering</option>
        </select>
        </p>
        <p>
            <input type="submit" value="Create Account" class="register-button">

        </p>
    </form>



</div>


</body>

</html>
