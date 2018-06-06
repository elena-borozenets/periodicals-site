
$(function () {
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
