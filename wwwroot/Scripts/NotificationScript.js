async function getNotifications() {
    // Va récupérer la méthode du controller salarie qui s'appelle getAllNotificationsByUser
    var url = '/salarie/getAllNotificationsByUser'
    request = fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    }
    )
    result = await request;
    // Le format Json permet de récupérer les infos des notifications
    json = await (result.json());
    // On appelle cette méthode pour afficher chaque notification
    showNotifications(json);
}

getNotifications();

// En argument, le Json récupéré dans la méthode précédente
function showNotifications(data) {
    // On cible la div qui va contenir les notifications
    var target = document.querySelector("#notifications");

    var notifStr = "";

    // On boucle sur la liste de notification
    for (var i = 0; i < data.length; i++) {
        // On met le classContext dans la classe pour se servir de Bootstrap
        // On met le messageContent dans le contenu de la div
        notifStr += `<div class="alert alert-${data[i].classContext}"> ${data[i].messageContent}  </div>`
    }
    // La cible va se faire ajouter le contenu Html de la variable notifStr
    target.innerHTML = notifStr;

}