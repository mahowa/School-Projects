<?php
include("partial.php");

$partial = new Partial("readme");
echo $partial->head("ReadMe");
?>
<style>
    #wrapper {
    padding-left: 250px;
    transition: all 0.4s ease 0s;
    }

    #sidebar-wrapper {
    margin-left: -250px;
    left: 250px;
    width: 250px;
    background: #000;
    position: fixed;
    height: 100%;
    overflow-y: auto;
    z-index: 1000;
    transition: all 0.4s ease 0s;
    }

    #wrapper.active {
    padding-left: 0;
    }

    #wrapper.active #sidebar-wrapper {
    left: 0;
    }

    #page-content-wrapper {
    width: 100%;
    }



    .sidebar-nav {
    position: absolute;
    top: 0;
    width: 250px;
    list-style: none;
    margin: 0;
    padding: 0;
    }

    .sidebar-nav li {
    line-height: 40px;
    text-indent: 20px;
    }

    .sidebar-nav li a {
    color: #999999;
    display: block;
    text-decoration: none;
    padding-left: 60px;
    }

    .sidebar-nav li a span:before {
    position: absolute;
    left: 0;
    color: #41484c;
    text-align: center;
    width: 20px;
    line-height: 18px;
    }

    .sidebar-nav li a:hover,
    .sidebar-nav li.active {
    color: #fff;
    background: rgba(255,255,255,0.2);
    text-decoration: none;
    }

    .sidebar-nav li a:active,
    .sidebar-nav li a:focus {
    text-decoration: none;
    }

    .sidebar-nav > .sidebar-brand {
    height: 65px;
    line-height: 60px;
    font-size: 18px;
    }

    .sidebar-nav > .sidebar-brand a {
    color: #999999;
    }

    .sidebar-nav > .sidebar-brand a:hover {
    color: #fff;
    background: none;
    }



    .content-header {
    height: 65px;
    line-height: 65px;
    }

    .content-header h1 {
    margin: 0;
    margin-left: 20px;
    line-height: 65px;
    display: inline-block;
    }

    #menu-toggle {
    text-decoration: none;
    }

    .btn-menu {
    color: #000;
    }

    .inset {
    padding: 20px;
    }

    @media (max-width:767px) {

    #wrapper {
    padding-left: 0;
    }

    #sidebar-wrapper {
    left: 0;
    }

    #wrapper.active {
    position: relative;
    left: 250px;
    }

    #wrapper.active #sidebar-wrapper {
    left: 250px;
    width: 250px;
    transition: all 0.4s ease 0s;
    }

    #menu-toggle {
    display: inline-block;
    }

    .inset {
    padding: 15px;
    }

    }

    .jumbotron {
        background-image: url("img/muss.jpg");
        background-size: cover;
        color: white;
        opacity: 0.6;
    }

</style>

    <div id="wrapper">

        <!-- Sidebar -->
        <div id="sidebar-wrapper">
            <nav id="spy">
                <ul class="sidebar-nav nav">
                    <li class="sidebar-brand">
                        <a href="#v7"><span class="fa fa-anchor solo">V7</span></a>
                    </li>
                    <li class="sidebar-brand">
                        <a href="#v6"><span class="fa fa-anchor solo">V6</span></a>
                    </li>
                    <li>
                        <a href="#v5" data-scroll>
                            <span class="fa fa-anchor solo">V5</span>
                        </a>
                    </li>
                    <li>
                        <a href="#v4" data-scroll>
                            <span class="fa fa-anchor solo">V4</span>
                        </a>
                    </li>
                    <li>
                        <a href="#v3" data-scroll>
                            <span class="fa fa-anchor solo">V3</span>
                        </a>
                    </li>
                    <li>
                        <a href="#v2" data-scroll>
                            <span class="fa fa-anchor solo">V2</span>
                        </a>
                    </li>
                    <li>
                        <a href="#v1" data-scroll>
                            <span class="fa fa-anchor solo">V1</span>
                        </a>
                    </li>
                </ul>
            </nav>
        </div>

        <!-- Page content -->
        <div id="page-content-wrapper">
            <div class="content-header">
                <h1 id="home">
                    <a id="menu-toggle" href="#" class="glyphicon glyphicon-align-justify btn-menu toggle">
                        <i class="fa fa-bars"></i>
                    </a>
                </h1>
            </div>

            <div class="container-fluid page-content inset" data-spy="scroll" data-target="#spy"  >
                <div class="row">

                    <div class="jumbotron text-center" >
                        <h1>ReadMe</h1>
                        <p>Matthew Howa</p>
                        <p><a href="http://github.com/uofu-cs4540/805396" class="btn btn-default">GitHub Repo</a></p>
                    </div>

                </div>
                <div class="row">
                    <div class="panel panel-danger">
                        <div class="panel-heading">V7</div>
                        <div class="panel-body" id="v7">
                            <strong>Summary</strong>
                            <p>
                                Username: director<br>
                                Password: Password1<br>
                                <br>
                                I implemented 3 charts: GPA's, Signed Forms, Advised By. <br>
                                You can access them once  you enter DGS by scrolling to the bottom of the screen and selecting the chart you wish to view
                                in the dropdown.<br>
                                <br>
                                Next I implemented a quick view summary of student progress. You can access this by pressing the blue magnifying button
                                on the students row of the table view.<br>
                                <br>
                                All the data comes from a db and is accessed via ajax.
                                <br><br>
                                The graphs I chose would help the DGS understand the break up of gpa's by year. It would help them see how many forms are
                                unfinished. It would also let them see how many students each advisor is advising. All these things would be very useful
                                however I think the quick view of the summary of the students would be the most useful.
                            </p>
                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V6</div>
                        <div class="panel-body" id="v6">
                            <strong>Summary</strong>
                            <ol>
                                <li>Navbar
                                    <ul>
                                        <li>Implemented the bootstrap nav bar</li>
                                        <li>Added Brand image</li>
                                        <li>Converted version links to a drop down</li>
                                        <li>Found on every page</li>
                                        <li>On the GradProgress Pages I have added the ability to login from the nav bar</li>
                                    </ul>
                                </li>
                                <li>Footer
                                    <ul>
                                        <li>Implemented the bootstrap sticky footer</li>
                                        <li>Had to hack the bootstrap in order for it to work</li>
                                        <li>Found on every page</li>
                                    </ul>
                                </li>
                                <li>Scroll Spy
                                    <ul>
                                        <li>Implemented the bootstrap spyscroll</li>
                                        <li>Proved to be a very difficult task</li>
                                        <li>Found on this page. (ReadMe)</li>
                                    </ul>
                                </li>
                                <li>Jumbo Tron
                                    <ul>
                                        <li>Implemented the bootstrap Jumbotron</li>
                                        <li>Updated the bacground image</li>
                                        <li>Found on this page. (ReadMe)</li>
                                    </ul>
                                </li>
                                <li>Glyphicons
                                    <ul>
                                        <li>Implemented the bootstrap Glyphicons</li>
                                        <li>Found on this page. And throughout</li>
                                    </ul>
                                </li>
                                <li>Tables
                                    <ul>
                                        <li>Implemented the bootstrap Tables</li>
                                        <li>Found on all the grad progress pages</li>
                                        <li>The tables found on the DGS overview page are responsive and clickable</li>
                                    </ul>
                                </li>
                                <li>Tooltips
                                    <ul>
                                        <li>Implemented the bootstrap tooltips</li>
                                        <li>Found on buttons edit/view/sign</li>
                                    </ul>
                                </li>
                                <li>Popover
                                    <ul>
                                        <li>Implemented the bootstrap popover</li>
                                        <li>Found on due progress form, Click colored checks to see  popover content</li>
                                    </ul>
                                </li>
                                <li>Grid
                                    <ul>
                                        <li>Used the bootstrap grid system</li>
                                        <li>Found on many pages but some good examples are the signin page and the Readme page</li>
                                    </ul>
                                </li>
                                <li>Alerts
                                    <ul>
                                        <li>Implemented bootstrap alert</li>
                                        <li>Found on sign in page when you enter the wrong username or password</li>
                                    </ul>
                                </li>
                                <li>Modals
                                    <ul>
                                        <li>Implemented bootstrap Modals</li>
                                        <li>You will see this when you sign in</li>
                                    </ul>
                                </li>
                                <li>Wells
                                    <ul>
                                        <li>Implemented bootstrap Wells</li>
                                        <li>You will see this on the sign in page</li>
                                    </ul>
                                </li>
                                <li>Carousel
                                    <ul>
                                        <li>Implemented bootstrap Carousel</li>
                                        <li>You will see this on the home page</li>
                                        <li>I already had custom css to make this effect but thought Id try out the bootstrap version... MUCH Easier</li>
                                    </ul>
                                </li>
                                <li>Image Circle
                                    <ul>
                                        <li>Implemented bootstrap image cropping</li>
                                        <li>You will see this on the home page</li>
                                    </ul>
                                </li>
                                <li>Panel
                                    <ul>
                                        <li>Implemented bootstrap Panel with heading</li>
                                        <li>This is in a panel with a heading</li>
                                    </ul>
                                </li>
                            </ol>
                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V5</div>
                        <div class="panel-body" id="v5">
                            <p>
                                <strong>Summary V5 -</strong>
                                In this assignment we were tasked with adding roles to our site. I have added some users in the db for
                                testing, You
                                will find their credentials displayed on the sign in page. For help completing this task I used the
                                class examples.
                                You may also add new users by registering but by default they are set to the lowest roll and are not
                                even considered
                                students yet. They are just users. I intend for the next release to give DGS the ability to update user
                                privleges.
                            </p>
                            <p>
                                The DGS can search by advisors or students and can view and sign student forms.
                            </p>
                            <p>
                                Advisor roles allow the user to see the students they are assigned to advise and are allowed view and to
                                sign forms of their students
                            </p>
                            <p>
                                Students are allowed to create new forms, view forms and see the status of their forms.
                            </p>
                            <p>
                                Because we are now allowing users to sign in and restricting certain pages based on roles I implemented
                                a
                                username password system. The password is salted and hased then saved in the database. Also for securtiy
                                we force
                                https on some pages of the site.
                            </p>
                            <P>
                                Finally whenever you go to the register page or sign in page you are forced to the secured https
                                protocol
                            </P>

                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V4</div>
                        <div class="panel-body" id="v4">
                            <p>
                                <strong>Summary V4 -</strong>
                                This assignment was huge. I really didn't realize it until I really started implementing everything. I
                                am turning this in a day late.
                                Ive actually been up all night so I hope you'll take it. Just a side not I started this over a week ago
                                but things kept adding up.
                            </p>
                            <p>
                                New User - Implemented a form that allows a student to be made and faculty member to be made. Any new
                                faculty members are
                                added their respective department and any new students are added to processing. The frontend does normal
                                html form checking
                                while the back end will make sure the required fields are there. If they are not they will be
                                pre-populated and notify you that you
                                are missing fields. This was the distinction for superior work.
                            </p>
                            <p>
                                Due Progress Form- Major over haul here. Using a bit of javascript I believe Ive significantly improved
                                my ui. I implemented
                                a sign form, view form, edit form, and new form. Signing the form will actually record in db but does
                                not have any other
                                features than that at the moment. I plan to implement some restriction to editing a signed form and
                                instead force
                                the user to create a new one. I also want to display that the form has been signed. Check out how
                                pressing save actually will save
                                the data to the db.
                            </p>
                            <p>
                                Navigation- I believe the way I set up navigation is intuitive and looks good. To start press projects
                                then select Due Progress form.
                                From here you are at the main page allowing you to select advisors or just students. You also now have
                                the option to create
                                a new user. Now you are given 4 options to select form in order to get to the form. VIEW/EDIT/SIGN/NEW
                            </p>
                            <p>
                                Additional Updates - Spent quite a bit of time making partials. They made my website feel much cleaner
                                and unified
                            </p>
                            <p>
                                Plan for future - On form add tool tips showing how many semesters needed for good/adequate. Also
                                signify what the student got by
                                making the opacity on the other checks lower.
                                -Next I want to display if the form has been signed
                            </p>
                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V3</div>
                        <div class="panel-body" id="v3">
                            <p>
                                <b>Summary V3</b> - This version we were asked to connect our site to a mySql database. I have done so
                                and also
                                made some changes to my ui and folder structure as directed by both the TA and the Professor
                            </p>
                            <p>
                                <b> UI choice</b> - My website was chosen to display during class. Although there were some good
                                comments
                                there was also plenty of improvements to be made. I have
                                tried to address these problems in this release and would like you to see my updates. First I changed
                                all
                                the green found on my site to red in order to be more themed and follow
                                our colors at the U. Next I got rid of the css that made font bigger as you hovered over it. Professor
                                Germain said it was distracting and I tend to agree with him. My original
                                though was it would make it stand out but its didn't work as I had hoped so removed it. Next I removed
                                the
                                background on my form because again professor germain said to be careful
                                about making stuff to cool. He said its probably not bad the first time but after looking at it multiple
                                times our eyes might play tricks on the user and like i said earlier be distracting. Also I did not
                                particularly
                                like my tables on my site so when looking at my peers sites I found much better looking tables. I credit
                                them
                                as such in the attribution section.
                            </p>
                            <p>
                                <b> Grading </b> - When grading please note I followed the guidelines of the assignment but also made
                                improvements where others have given me constructive criticism. I brought both V1 and V2 into
                                my repository instead of just sitting on the server in order to have better control over links and
                                stuff. I
                                also place the class examples in the repository for a similar reason. I spent a lot of time on ui. I was
                                going to dive
                                into the form some more but decided since our next version will be all about forms, I thought
                                demonstrating I am
                                connected to the db was good enough and I was also starting to get short on time.
                            </p>
                            <p>
                                <b> Testing</b> - To test I created a local db using MAMP in order to test. Then when it came to deploy
                                everything worked just swimmingly because I didn't have to change any code to get it to
                                work on the server.
                            </p>
                            <p>
                                <b> Attributions</b> - My attributions go out to some of my classmates. I decided to take a page from
                                Matt
                                Wilden who had some very visually apealing tables. They were found
                                <a href="http://uofu-cs4540-86.westus.cloudapp.azure.com/Projects/Grad_Progress/Controller/Advisor/students.php?id=1">
                                    here
                                </a>.

                            </p>
                            <p>
                                This is one day late please accept my submission for v-3 1 day late.
                            </p>
                            <p>
                                <b> Peer Review</b> - The group I reviewed for this project was the pair Brandon Tobin and Anne Aoki
                            </p>
                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V2</div>
                        <div class="panel-body" id="v2">
                            <p>
                                <b>Summary V2</b> - This project is the exact same as the first except this ti\
                                me we use mvc. We also have introduced the concept of advisors and departments. \
                                Also I correctly implemented the .htaccess file in order to get redirect the browser to the controller.
                                The
                                controller then grabs the model which is used to populate\
                                the view. The controller is in charge of grabbing and interpreting any \
                                data sent from the browser.

                                <b> UI choice</b> - Due to a stain on time there wasnt to many UI changes. However I did change the
                                color
                                scheme to better reflect the University of Utah
                            </p>
                            <p>
                                <b> Grading </b> - When grading please look at my views and how I chose to reuse code. <br/>
                                There is a lot of mock data and some does not match up due to me being semi random about it but it
                                should be easy
                                to see that when we do grab data from a database that you will easily be able to integrate it.
                            </p>
                            <p>
                                <b> Testing</b> - Testing for this assignment was very rough but as soon as I got the hang of mvc it was
                                as easy as using vardump and just simply refreshing the web page
                            </p>
                            <p>
                                <b> Attributions</b> - Did not reference any websites on this assignment. I just used the class examples
                                as a template and went from there.
                            </p>


                            <p><b>Note</b> - I turned this in late but before midnight :)</p>
                        </div>
                    </div>


                    <div class="panel panel-danger">
                        <div class="panel-heading">V1</div>
                        <div class="panel-body" id="v1">
                            <p>
                                <b>Summary V1</b> - This project mocks up the for that will be used to keep
                                track of graduate student progress. It does not use a
                                db therefore all data is static.

                                I chose to use divs to make a table because when resizing
                                so does the table. I also chose large controls and did my best
                                to keep the color scheme. Unfortunatley Im partially color blind
                                so please dont dock me for that.

                                I really spent most of my time with css and have grown to really like it.
                                I dedicated a good chunck of time to the nav bar.

                                Testing strat was to refresh the browser. Not much else

                                http://www.w3schools.com/
                                https://codepen.io/dudleystorey/pen/ehKpi
                        </div>
                    </div>

                </div>


            </div>

        </div>

    </div>




<script>

    /*Menu-toggle*/
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("active");
    });

    /*Scroll Spy*/
    $('body').scrollspy({ target: '#spy', offset:80});

    /*Smooth link animation*/
    $('a[href*=#]:not([href=#])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') || location.hostname == this.hostname) {

            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top-50
                }, 1000);
                return false;
            }
        }
    });

</script>

<?php
echo $partial->foot();
?>