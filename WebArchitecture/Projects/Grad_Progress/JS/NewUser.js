/**
 * Created by mahowa on 2/15/2016.
 */

$(document).ready(function(){
    switchToStudent();
});
//Show student options
function switchToStudent () {
    var semester = document.getElementById('semester');
    semester.style.display = 'inline';
    semester.required = true;
    var degree = document.getElementById('degree');
    degree.style.display = 'inline';
    degree.required = true;
    var department = document.getElementById('department');
    department.style.display = 'none';
    department.required = false;
}
//Show faculty options
function switchToFaculty () {
    var semester = document.getElementById('semester');
    semester.style.display = 'none';
    semester.required = false;
    var degree = document.getElementById('degree');
    degree.style.display = 'none';
    degree.required = false;
    var department = document.getElementById('department');
    department.style.display = 'inline';
    department.required  = true;
}


/**
 * Note: by returning FALSE we DISABLE THE SUBMIT
 *
 *       forgetting to return false will result in the form being submitted
 *       regardless of error echecking
 *
 */
function checkForm(form)
{
    if(form.username.value == "")
    {
        alert("Error: Username cannot be blank!");
        form.username.focus();
        return false;

    }

    re = /^\w+$/;

    if(!re.test(form.username.value))
    {
        alert("Error: Username must contain only letters, numbers and underscores!");
        form.username.focus();
        return false;
    }

    if (form.pwd1.value == "")
    {
        alert("Error: Must have a password");
        form.pwd1.focus();
        return false;
    }

    if (form.pwd1.value != form.pwd2.value)
    {
        alert("Error: Passwords don't match");
        form.pwd1.focus();
        return false;
    }

    if(form.pwd1.value.length < 6)
    {
        alert("Error: Password must contain at least six characters!");
        form.pwd1.focus();
        return false;
    }

    if(form.pwd1.value == form.username.value)
    {
        alert("Error: Password must be different from Username!");
        form.pwd1.focus();
        return false;
    }

    re = /[0-9]/;
    if(!re.test(form.pwd1.value))
    {
        alert("Error: password must contain at least one number (0-9)!");
        form.pwd1.focus();
        return false;
    }


    re = /[a-z]/;
    if(!re.test(form.pwd1.value))
    {
        alert("Error: password must contain at least one lowercase letter (a-z)!");
        form.pwd1.focus();
        return false;
    }


    re = /[A-Z]/;
    if(!re.test(form.pwd1.value))
    {
        alert("Error: password must contain at least one uppercase letter (A-Z)!");
        form.pwd1.focus();
        return false;
    }


    return true;

}
