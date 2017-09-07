<?php

$target_dir = "uploads/";
$dirpath = realpath(dirname(getcwd()));
$target_file = $dirpath . DIRECTORY_SEPARATOR . "TeeDesigner" . DIRECTORY_SEPARATOR . "uploads" .DIRECTORY_SEPARATOR . basename($_FILES["file"]["name"]);

$uploadOk = 1;
$imageFileType = pathinfo($target_file,PATHINFO_EXTENSION);

// Check if image file is a actual image or fake image

if(!empty($_FILES)) {
    move_uploaded_file($_FILES["file"]["tmp_name"], $target_file);
}


//$content = "some text here";
//$fp = fopen($_SERVER['DOCUMENT_ROOT'] . "/myText.txt","wb");
//fwrite($fp,$content);
//fclose($fp);$myfile = fopen("testfile.txt", "w")
?>
