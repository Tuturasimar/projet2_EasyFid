$(document).ready(() => {
    $("#addActivity").click(function () {
        var rowCount = parseInt($("#total").val());
        rowCount++;
        $("#total").val(rowCount);

        var divACloner = document.getElementsByClassName('hidden');

        var activityToAdd = divACloner[0].cloneNode(true);
        activityToAdd.className = "";


        const divToInclude = document.getElementById("newActivity");
        divToInclude.appendChild(activityToAdd);

    });

    $(document).on('click', '#deleteActivity',function () {

        var rowCount = parseInt($("#total").val());
        rowCount--;
        $("#total").val(rowCount);

        $(this).closest('#activityHidden').remove();
    });
})
