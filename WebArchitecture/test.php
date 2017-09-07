<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">


    <meta name="AUTHOR"      content="Matthew J. Howa"/>
    <meta name="keywords"    content="HTML, Template"/>
    <meta name="description" content="Landing page for CS4140- Web Architecture"/>

    <link rel="icon" href="img/favicon.ico">

    <title>Sticky Footer Template for Bootstrap</title>
    <link rel='stylesheet' href='bootstrap/css/bootstrap.min.css' type='text/css'/>
    <link rel='stylesheet' href='styles/required.css' type='text/css'/>
    <script src="https://code.jquery.com/jquery-2.1.3.min.js"></script>

<!--    <script src='js/jquery.js' type='text/javascript'> </script>-->
    <script src='bootstrap/js/bootstrap.min.js' type='text/javascript'> </script>

</head>

<body>

<nav class="navbar navbar-default navbar-fixed-top navbar-inverse">
    <div class="container-fluid">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <img class="navbar-brand" src="img/u-logo.png"/>
        </div>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav">
                <li class="active"><a href="#">Home <span class="sr-only">(current)</span></a></li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Projects <span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="Projects/Grad_Progress/">Graduate Student Due Process Form</a></li>
                    </ul>
                </li>
                <li><a href="#">Class Examples</a></li>
                <li><a href="#">ReadMe</a></li>
                <li><a href="#">Schema</a></li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Versions <span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="V1/">V1</a></li>
                        <li><a href="V2/">V2</a></li>
                        <li><a href="V3/">V3</a></li>
                        <li><a href="V4/">V4</a></li>
<!--                        <li><a href="V5/">V5</a></li>-->
                    </ul>
                </li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="https://github.com/uofu-cs4540/805396"><img src="img/gitIcon.png" height="25" width="25"/></a></li>
            </ul>
        </div><!-- /.navbar-collapse -->
    </div><!-- /.container-fluid -->
</nav>
<div id="wrap">
    <div class="container">

        <div class="starter-template">
            <h1>Bootstrap starter template</h1>
            <p class="lead">Use this document as a way to quickly start any new project.<br> All you get is this text and a mostly barebones HTML document.</p>
        </div>

    </div><!-- /.container -->
</div>
<div id="footer">
    <div class="container">
        <div class="col-md-4 col-md-offset-4">
            <p class="text-muted">CS 4540 - Web Software Architecture - Spring 2016</p>
            <p class="text-muted text-center">© Matthew J. Howa 2016</p>
        </div>
    </div>
</div>

</body>
</html>
