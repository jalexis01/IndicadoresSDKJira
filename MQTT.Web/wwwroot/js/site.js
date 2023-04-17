// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var filtersData;
var dataGridSave;


$(document).ready(function(){

    if(location.pathname != "/Identity/Account/Login"){
        $.ajax({
            type: "POST",
            dataType: "json",
            url: '/Messages/BackupSearch',
        }).then(response => JSON.parse(JSON.stringify(response)))
            .then(data => {
                createElemntsTimesBackup();
                drodownDataSearchBackup(data, 'customName');
            })
            .catch(error => {
                location.href = location.origin + "/Identity/Account/Login";
            })
            .then(response => console.log('Success:', response));
    }
});

function Eys(){
    $("#navbar").addClass("footer-eys");
    $("#footer").addClass("navbar-eys");
    $("#loader").addClass("loader-color-eys");
}

function Manatee(){
    $("#navbar").addClass("navbar-manatee");
    $("#footer").addClass("footer-manatee");
    $("#loader").addClass("loader-color-manatee");

    const buttonsRadius = document.querySelectorAll("#button-primary-radius");
    const buttons = document.querySelectorAll("#button-primary");
    const dropdown = document.querySelectorAll("#dropdownInformationButton");
    const buttonsCreate = document.querySelectorAll('#button-create');
    if (buttons != undefined) {
        buttons.forEach(element => {
            element.className = "text-white bg-[#FF9119] hover:bg-[#FF9119]/80 focus:ring-4 focus:outline-none focus:ring-[#FF9119]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:hover:bg-[#FF9119]/80 dark:focus:ring-[#FF9119]/40 mr-2 mb-2";
        })
    }

    if (dropdown != undefined) {
        dropdown.forEach(element => {
            element.className = "text-white bg-[#FF9119] hover:bg-[#FF9119]/80 focus:ring-4 focus:outline-none focus:ring-[#FF9119]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:hover:bg-[#FF9119]/80 dark:focus:ring-[#FF9119]/40 mr-2 mb-2 organitation-menu font-style-app";
        })
    }

    if (buttonsRadius != undefined) {
        buttonsRadius.forEach(element => {
            element.className = "text-white bg-[#FF9119] hover:bg-[#FF9119]/80 focus:outline-none focus:ring-4 focus:ring-[#FF9119]/50 font-medium rounded-full text-sm px-5 py-2.5 text-center mr-2 mb-2 dark:bg-[#FF9119]/60 dark:hover:bg-[#FF9119]/80 dark:focus:ring-[#FF9119]/40 font-style-app";
        })
    }

    if (buttonsCreate != undefined) {
        buttonsCreate.forEach(element => {
            element.className = "text-white bg-[#FF9119] hover:bg-[#FF9119]/80 focus:ring-4 focus:outline-none focus:ring-[#FF9119]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:hover:bg-[#FF9119]/80 dark:focus:ring-[#FF9119]/40 mr-2 mb-2 create-button";
        })
    }


}

function bodyLogin(){
    switch(dataEmpresa){
        case 'Manatee':
            $("body").addClass("class-body-manatee");
            $("#logo").attr('src','../../img/manatee-logo.png')
            break;
        case 'Eys':
            $("body").addClass("class-body-eys");
            $("#logo").attr('src','../../img/eys-logo.png')
            break;
        default:
            $("body").addClass("class-body-eys");
            $("#logo").attr('src','../../img/eys-logo.png')
            break;
    }
}

function selectSpace(){
    switch(dataEmpresa){
        case 'Manatee':
            Manatee();
            break;
        case 'Eys':
            Eys();
            break;
        default:
            Eys();
            break;
    }
}

/** 
function setIdUser(){
    if((sessionStorage.getItem('idUser') == undefined || sessionStorage.getItem('idUser') == null) && location.pathname != "/Identity/Account/Login"){
        $.ajax({
            type: "GET",
            dataType: "json",
            url: '/Messages/UserDataId',
          }).then(response => JSON.parse(JSON.stringify(response)))
          .then(data => {
            sessionStorage.setItem('idUser', data.idUser);
          })
          .catch(error => {
            location.href = location.origin + "/Identity/Account/Login";
          })
          .then(response => console.log('Success:', response));
    }
}*/

selectSpace();