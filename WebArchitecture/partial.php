<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 2/17/2016
 * Time: 4:55 PM
 */


class Partial{


    private $CSS;
    private $SCRIPTS;
    private $NavLinks;
    public $role;
    private $page;

    //Determines the extra includes to include in every header
    public function __construct($page)
    {

        $this->page = $page;
        $this->navlink_setup();
        switch($page){
            case "new_user":
                $this->SCRIPTS .= "<script src='../../JS/NewUser.js' type='text/javascript'> </script>";
                $this->CSS .="<link rel='stylesheet' href='../../Style/Register.css' type='text/css'/>";
                $this->redirectToHTTPS();
                //$this->changeSessionID ();
                //return;
                break;
            case "sign_in":
                //$this->SCRIPTS .= "<script src='../../JS/NewUser.js' type='text/javascript'> </script>";
                $this->CSS .="<link rel='stylesheet' href='../../Style/Register.css' type='text/css'/>";
                $this->redirectToHTTPS();
                //$this->changeSessionID ();
                //return;
                break;

            case "dForm":
                $this->SCRIPTS .= "<script src='../../JS/DueProgress.js' type='text/javascript'> </script>";
                $this->CSS .="<link rel='stylesheet' href='../../Style/form.css' type='text/css'/>";
                break;

            case "home":
                $this->CSS .= "<link rel='stylesheet' href='styles/home-page.css' type='text/css'/>";

                break;
            default:
                //      this->startSession();
                break;
        }
        //$this->startSession();

        // $this->changeSessionID();
    }



    //Partial containing the header and navbar for each file
    public function head($title){
        return  "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">
                <html lang='en'>
                <head>
                    <meta charset='utf-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>

                    <meta name='AUTHOR'      content='Matthew J. Howa'/>
                    <meta name='keywords'    content='HTML, Template'/>
                    <meta name='description' content='Landing page for CS4140- Web Architecture'/>

                    <link rel='icon' href='img/favicon.ico'>

                    <title>$title</title>
                    <link rel='stylesheet' href='styles/required.css' type='text/css'/>
                    <link rel='stylesheet' href='bootstrap/css/bootstrap.min.css' type='text/css'/>
                    {$this->CSS}

                    {$this->SCRIPTS}
                    <script src='https://code.jquery.com/jquery-2.1.3.min.js'></script>
                    <script src='bootstrap/js/bootstrap.min.js' type='text/javascript'> </script>
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
                            <img class='navbar-brand' src='img/u-logo.png'/>
                        </div>

                        <!-- Collect the nav links, forms, and other content for toggling -->
                        <div class='collapse navbar-collapse' id='bs-example-navbar-collapse-1'>
                            <ul class='nav navbar-nav'>
                                $this->NavLinks
                            </ul>
                            <ul class='nav navbar-nav navbar-right'>
                                <li><a href='https://github.com/uofu-cs4540/805396'><img src='img/gitIcon.png' height='20' width='20'/></a></li>
                            </ul>
                        </div><!-- /.navbar-collapse -->

                    </div><!-- /.container-fluid -->
                </nav>
                ";
    }

    //Footer partial to include in each file
    public function foot(){
        return "
                    <div id='footer'>
                            <div class='col-md-4 col-md-offset-4'>
                                <p class='text-muted text-center'>CS 4540 - Web Software Architecture - Spring 2016</p>
                                <p class='text-muted text-center'>© Matthew J. Howa - 2016</p>
                            </div>
                    </div>
                </body>
            </html>
        ";
    }

    private function navlink_setup(){
        $home ="<li><a href='index.php'>Home</a></li>";
        $project ="<li class='dropdown'>
                        <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>Projects <span class='caret'></span></a>
                        <ul class='dropdown-menu'>
                            <li><a href='Projects/Grad_Progress/Controller/Main/main.php'>Graduate Student Due Process Form</a></li>
                            <li><a href='Projects/Tetris/tetris.php'>Tetris</a></li>
                            <li><a href='Projects/TeeDesigner/index.php'>Tee Designer</a></li>
                        </ul>
                    </li>";
        $classExample ="<li><a href='../Class_Examples/'>Class Examples</a></li>";
        $readme="<li><a href='README.php'>ReadMe</a></li>";
        $schema= " <li><a href='schema.php'>Schema</a></li>";
        $version="<li class='dropdown'>
                        <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>Versions <span class='caret'></span></a>
                        <ul class='dropdown-menu'>
                            <li><a href='V1/'>V1</a></li>
                            <li><a href='V2/'>V2</a></li>
                            <li><a href='V3/'>V3</a></li>
                            <li><a href='V4/'>V4</a></li>
                            <li><a href='V5/'>V5</a></li>
                            <li><a href='V6/'>V6</a></li>
                            <li><a href='V7/'>V7</a></li>
                            <li><a href='#'>V8</a></li>
                        </ul>
                    </li>";

        switch($this->page){
            case "new_user":

                break;
            case "sign_in":

                break;

            case "dForm":

                break;

            case "home":
                $home ="<li class='active'><a href='#'>Home<span class='sr-only'>(current)</span></a></li>";
                break;
            case "readme":
                $readme="<li  class='active'><a href='README.php'>ReadMe<span class='sr-only'>(current)</span></a></li>";
                break;
            case "schema":
                $schema= " <li  class='active'><a href='schema.php'>Schema<span class='sr-only'>(current)</span></a></li>";
                break;
            default:
                //      this->startSession();
                break;
        }
        $this->NavLinks = $home . $project  . $readme . $schema ;
    }

}
