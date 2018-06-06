
$(function () {
    $("#searchUser").keyup(function () {
        var search = $("#searchUser").val();
        $.ajax({
            type: "POST",
            //url: "search",
            data: { "search": search },
            url: "SearchUser",
            cache: false,
            success: function (response) {
                $("#resSearchUser").html(response);
            }
        });
        return false;
    });
});
