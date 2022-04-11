
var show_sidebar = false;

function toggle_sidebar() {
    if (show_sidebar == false) { document.getElementById("sidebar").style.left = "0"; show_sidebar = true; }
    else { document.getElementById("sidebar").style.left = "-320px"; show_sidebar = false; }
}