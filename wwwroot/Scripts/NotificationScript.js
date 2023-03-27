async function getNotifications() {
    var url = '/salarie/getAllNotificationsByUser'
    request = fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
          //  'X-CSRFToken': csrftoken,
        }
    }
    )
   // console.log('productId:', productId)
    result = await request;
    json = await (result.json());
    showNotifications(json);
}

getNotifications();

function showNotifications(data) {
    var target = document.querySelector("#notifications");

    var notifStr = "";

    for (var i = 0; i < data.length; i++) {
        notifStr += `<div class="alert alert-${data[i].classContext}"> ${data[i].messageContent}  </div>`
    }
    target.innerHTML = notifStr;

}