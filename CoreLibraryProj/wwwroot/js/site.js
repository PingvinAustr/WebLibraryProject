

//Function for Book Index view. 
function RestoreFilters() {
    document.getElementById("parameter1").value = "";
    document.getElementById("droppar_rubrics").value = "";
    document.getElementById("droppar_authors").value = "";

}


//Function for Book Details view.
function ChangeBookLanguage() {

    //Hiding all language divs
    for (var i = 0; i < 100; i++) {
        try { document.getElementById("lan" + i).style.display = "none"; }
        catch {}
    }

    //Making the chosen language div visible
    language_id = document.getElementById("language_front_dropdown").value;
    document.getElementById("lan" + language_id).style.display = "block";
}
