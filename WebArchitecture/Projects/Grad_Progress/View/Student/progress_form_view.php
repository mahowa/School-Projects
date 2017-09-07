<?php

/*
*               Matthew J. Howa
*                   U0805396
*             University of Utah
*               2016 - Web Architecture
*           This is the View Class For my Due progress form. I echo html in a php block in order to insert variables into it
*               this is a better approach then using < ?php ?>  blocks because it requires less code and better code management
*/




if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH']) && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
    echo $partial->head("Due Progress Form");
    echo $form->html();
}
else{
    echo $form->formatMilestonesHtml();
    $form->description();
}


?>

<script>
    $(document).ready(function(){
        $('[data-toggle="popover"]').popover();

    });
</script>

<?php
if(!(!empty($_SERVER['HTTP_X_REQUESTED_WITH']) && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')) {
    echo $partial->foot();
}
