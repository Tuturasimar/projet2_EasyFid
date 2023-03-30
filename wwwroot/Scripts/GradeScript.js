function changeStars(arg, turn) {
    var value = parseInt(arg.substring(arg.indexOf("-") + 1)) + 1;    // on récupère ici le numéro de l'étoile (de 1 à 5)
    var category = arg.substring(0, arg.indexOf("-"));                // on récupère ici une string qui contient la catégorie affiliée à la note

    // Pour la compréhension, ouvrez la console du navigateur, puis onglet console et cliquez sur des étoiles
    console.log("Valeur : ", value,"| Catégorie : ", category); 
    // le console.log sera à supprimer pour la suite, on ne les garde pas longtemps.

    // On récupère l'élément input caché qui correspond à la bonne mission et la bonne catégorie
    var input = document.querySelector(`#${category}${turn}`); 
    input.value = value; // On donne comme valeur à l'input caché, la valeur récupérée au début

    // On récupère l'élément le plus proche de notre input caché qui dispose d'une id correspondant à la concaténation de la catégorie et de Grade
    var parentDiv = input.closest(`#${category}Grade`);
    // On récupère tous les enfants de cette div. Les enfants sont (entre autre) les 5 étoiles déjà présentes.
    var childStar = parentDiv.children;

    // On boucle sur les enfants
    // On commence à 1 car l'élément à 0 est un label
    // On termine avant le dernier élément car c'est l'input caché
    // Donc, cette boucle va toujours boucler sur les 5 éléments qui sont les étoiles
    for (var i = 1; i < (childStar.length) - 1; i++) {
        // Même logique que dans la vue partielle.
        // Si la valeur est inférieure ou égale, on change la classe de l'élément récupéré en fa fa-star goldStar, ce qui correspond à une étoile pleine dorée
        if (i <= value) {
            childStar[i].className = "fa fa-star goldStar";
        } else {
        // Sinon, on change la classe en fa fa-star-o redStar, ce qui correspond à une étoile vide rouge
            childStar[i].className = "far fa-star redStar";
        }
    }
}
