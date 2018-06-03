
    $(function(){
        $("#search").keyup(function () {
            var search = $("#search").val();
            $.ajax({
                type: "POST",
                //url: "search",
                data: { "search": search },
                url: "Home/Search",
                cache: false,
                success: function (response) {
                    $("#resSearch").html(response);
                }
            });
            return false;
        });
    });


    var col = document.getElementsByClassName("genderRadio");
    for (let i = 0; i < col.length; i++) {
        col[i].onclick = a;
    }