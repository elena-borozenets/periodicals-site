
$(function () {
    $("#search").keyup(function () {
        var search = $("#search").val();
        $.ajax({
            type: "POST",
            //url: "search",
            data: { "search": search },
            url: "Edition/Search",
            cache: false,
            success: function (response) {
                $("#resSearch").html(response);
            }
        });
        return false;
    });
});
