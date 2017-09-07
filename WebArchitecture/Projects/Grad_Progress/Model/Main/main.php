<?php
/**
 * Created by PhpStorm.
 * User: mahowa
 * Date: 2/18/2016
 * Time: 1:17 PM
 */


class main{
    public $html;
    public function __construct(){

        //Director of graduate studies - Viewable
        if(isset($_SESSION['UserName']) && $_SESSION["Role"]==2) {
            $this->html = "
                    <div class='jumbotron text-center'>
                        <div class='well well-lg'>
                            <a href='../../Controller/DGS/overview.php' class='btn btn-lg btn-link'>Enter DGS</a>

                        </div>
                    </div>";
        }
        //Faculty and students - Not Viewable
        else if(isset($_SESSION['UserName'])) {
            $this->html = "
                    <div class='jumbotron text-center'>
                        <div class='well well-lg'>
                            You do not have permission to see this page
                        </div>
                    </div>";
        }
        //Not signed in - Not Viewable
        else{
            $this->html = "

                    <div class='jumbotron text-center'>
                        <div class='well well-lg'>
                            You must be signed in to view this page
                        </div>
                    </div>";
        }

    }



}