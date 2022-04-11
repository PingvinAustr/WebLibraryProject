
let current_theme = 'day';

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


//Function for Book Index View
function ToggleTheme() {

    if (current_theme == 'day') {
        document.getElementById('toggle_ball').style.marginLeft = "0px";
        current_theme = 'night';


        var matches = document.querySelectorAll(".search_header, .results_body, .select-css, .search_bar, .results_body_header, h1, .book_desc, body, .logout_group");
        matches.forEach(item => item.classList.add("night"));     


        document.getElementById('moon').src = "/lib/layout-img/active_moon.svg";
        document.getElementById('sun').src = "/lib/layout-img/inactive_sun.svg";
    }
    else {
        document.getElementById('toggle_ball').style.marginLeft = "45px";
        current_theme = 'day';
        document.getElementById('moon').src = "/lib/layout-img/inactive_moon.svg";
        document.getElementById('sun').src = "/lib/layout-img/active_sun.svg";

        var matches = document.querySelectorAll(".search_header, .results_body, .select-css, .search_bar, .results_body_header, h1, .book_desc, body, .logout_group");
        matches.forEach(item => item.classList.remove("night"));


    }








}

let small_first = true;
//Pretty movement function login page
function LoginPageToggleBlocks() {

    

    if (small_first==true) {
        document.getElementById("small_child").style.left = "66.666667%";
        document.getElementById("small_child").style.borderRadius = "0 40px 40px 0";
        document.getElementById("big_child").style.left = "-33.333334%";
        document.getElementById("big_child").style.borderRadius = "40px 0 0 40px";
        small_first = false;
       
    }
    else {
        document.getElementById("small_child").style.left = "0px";
        document.getElementById("small_child").style.borderRadius = "40px 0 0 40px";
        document.getElementById("big_child").style.left = "0px";
        document.getElementById("big_child").style.borderRadius = "0 40px 40px 0";
        small_first = true;
        console.log(small_first)
    }
}
