<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">
<html lang='en'>
<head>
    <meta charset='utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>

    <meta name='AUTHOR'      content='Matthew J. Howa'/>
    <meta name='keywords'    content='HTML, Template'/>
    <meta name='description' content='Landing page for CS4140- Web Architecture'/>

    <link rel='icon' href='../../img/favicon.ico'>

    <title>Tetris</title>
    <link rel='stylesheet' href='../../styles/required.css' type='text/css'/>
    <link rel='stylesheet' href='../../bootstrap/css/bootstrap.min.css' type='text/css'/>

    <script src='https://code.jquery.com/jquery-2.1.3.min.js'></script>
    <script src='../../bootstrap/js/bootstrap.min.js' type='text/javascript'> </script>
    <script src="pixi.js"  type="application/javascript"></script>

</head>

<body>

<nav class='navbar navbar-default navbar-fixed-top navbar-inverse'>
    <div class='container-fluid'>

        <!-- Brand and toggle get grouped for better mobile display -->
        <div class='navbar-header'>
            <button type='button' class='navbar-toggle collapsed' data-toggle='collapse' data-target='#bs-example-navbar-collapse-1' aria-expanded='false'>
                <span class='sr-only'>Toggle navigation</span>
                <span class='icon-bar'></span>
                <span class='icon-bar'></span>
                <span class='icon-bar'></span>
            </button>
            <img class='navbar-brand' src='../../img/u-logo.png'/>
        </div>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class='collapse navbar-collapse' id='bs-example-navbar-collapse-1'>
            <ul class='nav navbar-nav'>
                <li ><a href='../../'>Home<span class='sr-only'>(current)</span></a></li><li class='dropdown'>
                    <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>Projects <span class='caret'></span></a>
                    <ul class='dropdown-menu'>
                        <li><a href='../../Projects/Grad_Progress/Controller/Main/main.php'>Graduate Student Due Process Form</a></li>
                        <li class='active'><a href='../../Projects/Tetris/tetris.php'>Tetris</a></li>
                        <li><a href='../../Projects/TeeDesigner/index.php'>Tee Designer</a></li>
                    </ul>
                </li>
                <li><a href='../../README.php'>ReadMe</a></li>
                <li><a href='../../schema.php'>Schema</a></li>
            </ul>
            <ul class='nav navbar-nav navbar-right'>
                <li><a href='https://github.com/uofu-cs4540/805396'><img src='../../img/gitIcon.png' height='20' width='20'/></a></li>
            </ul>
        </div><!-- /.navbar-collapse -->

    </div><!-- /.container-fluid -->
</nav>

<div class="container center-block" >
    <div class="row">
        <div class="col-md-6 " id="tetris" >
            <div id="playbtn" class="well">
                <h3>Controls</h3>
                <ul>
                    <li><strong>ArrowDown</strong> - Move Down</li>
                    <li><strong>Space</strong> - Move to bottom</li>
                    <li><strong>ArrowLeft/ArrowRight</strong> - Move Horizontally</li>
                    <li><strong>ArrowUp</strong> - Rotate</li>
                </ul>
                <button id="playnow"  class="btn btn-primary" data-toggle="collapse" data-target="#lbdiv"  onclick="loadTetris()" style="margin-top: 80px">Play Tetris</button>
            </div>

        </div>
        <div class="col-md-6 " >
            <button class="btn btn-default" data-toggle='collapse' data-target='#lb_div' aria-expanded='false'>
                ...
            </button>
            <div id="lb_div" class="collapse in">
                <table class="table table-responsive" style="min-height: 200px" id="leaderboard"> </table>
            </div>
        </div>

    </div>
</div>

<!-- Game Over -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog">

        <!-- Submit Score-->
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Game Over</h4>
            </div>
            <div class="modal-body">
                <p>Enter your name to submit your score.</p>
                <input class="form-control" type="text"  id="name" placeholder="Enter Name" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal" onclick="submitscore()">Submit</button>
            </div>
        </div>
    </div>
</div>



<div id='footer'>
    <div class='col-md-4 col-md-offset-4'>
        <p class='text-muted text-center'>CS 4540 - Web Software Architecture - Spring 2016</p>
        <p class='text-muted text-center'>© Matthew J. Howa - 2016</p>
    </div>
</div>

<script src="tetris-clean.js" type="application/javascript"></script>
</body>
</html>
