// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {

    $('#mainContent').on('click', '.navigation a', function () {
        var url = $(this).attr('href');

        var guess = $('input[name="choice"]:checked').val();
        //if (guess != undefined)
        //    url += ("&input=" + guess) // AFTER MESSING WITH THE URL, RENDERING INDETERMINATE ISSUES ARISE

        $('#mainContent').load(url);

        return false; // WITHOUT RETURNING FALSE CONTROLLER'S CALL OCCURS TWICE
    })

});