$(document).ready(() => {
    $("#addActivity").click(function () {
        var rowCount = parseInt($("#total").val());
        rowCount++;
        $("#total").val(rowCount);

        var divACloner = document.getElementById('activityHidden');

        var activityToAdd = divACloner.cloneNode(true);
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
