

var xd = $(document).ready(function() {

    $('.show').click(function() {
        
        $(this).next().toggle();
        var visible = $(this).next().is(":visible");
        
if(visible){
$('a',this).html('Hide details');
$(this).html("Hide");

}else{
$('a',this).html('Show details');
$(this).html("Details");

}
        
    });

    
});


