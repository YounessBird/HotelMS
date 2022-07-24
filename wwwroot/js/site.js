// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function cat_highlight_row() {
  var table = document.getElementById("catTable");
  var rows = table.getElementsByTagName("tr");
  var inputName = document.getElementById("InName");
  var inputRemark = document.getElementById("InRemarks");
  var inputId = document.getElementById("CatId");
  var saveButton = document.getElementById("savebtn");
  table.onclick = function (e) {
    var rowSelected = e.target.parentElement;
    for (var i = 0; i < rows.length; i++) {
      if (rows[i].className == "selected") {
        if (rows[i].className == rowSelected.className) {
          rows[i].classList.remove("selected");
          inputName.value = "";
          inputRemark.value = "";
          inputId.value = "";
          saveButton.style.display = "block";
          return;
        }
        rows[i].classList.remove("selected");
      }
    }

    rowSelected.className = "selected";
    // hide the save button
    saveButton.style.display = "none";

    Array.from(rowSelected.cells, (el) => {
      switch (el.id) {
        case "catName":
          inputName.value = el.innerText;
          return;
        case "catRemark": {
          inputRemark.value = el.innerText;
          return;
        }
        case "catIdCell": {
          inputId.value = el.innerText;
          return;
        }
        default:
          break;
      }
    });
  };
}
function room_highlight_row() {
  var table = document.getElementById("roomTable");
  var rows = table.getElementsByTagName("tr");
  var inputName = document.getElementById("InName");
  var inputCategory = document.getElementById("secCategory");
  var inputLocation = document.getElementById("InLocation");
  var inputCost = document.getElementById("InCost");
  var inputRemark = document.getElementById("InRemark");
  var inputStatus = document.getElementById("secStatus");
  var inputId = document.getElementById("RoomId");
  var saveButton = document.getElementById("savebtn");

  table.onclick = function (e) {
    var rowSelected = e.target.parentElement;
    for (var i = 0; i < rows.length; i++) {
      if (rows[i].className == "selected") {
        if (rows[i].className == rowSelected.className) {
          rows[i].classList.remove("selected");
          inputName.value = "";
          inputCategory.value = "";
          inputLocation.value = "";
          inputCost.value = "";
          inputStatus.value = "";
          inputRemark.value = "";
          inputId.value = "";
          saveButton.style.display = "block";
          return;
        }
        rows[i].classList.remove("selected");
      }
    }
    // hide the save button
    saveButton.style.display = "none";

    rowSelected.className = "selected";

    Array.from(rowSelected.cells, (el) => {
      switch (el.id) {
        case "RName":
          inputName.value = el.innerText;
          return;
        case "RCategory": {
          inputCategory.value = el.dataset.id;
          return;
        }
        case "RLocation": {
          inputLocation.value = el.innerText;
          return;
        }
        case "RoomId": {
          inputId.value = el.innerText;
          return;
        }
        case "RRemark": {
          inputRemark.value = el.innerText;
          return;
        }
        case "RCost": {
          inputCost.value = el.innerText;
          return;
        }
        case "RStatus": {
          inputStatus.value = el.innerText == "Available" ? 0 : 1;
          return;
        }
        default:
          break;
      }
    });
  };
}

function user_highlight_row() {
  var table = document.getElementById("userTable");
  var rows = table.getElementsByTagName("tr");
  var inputName = document.getElementById("InName");
  var inputPhone = document.getElementById("InPhone");
  var inputGender = document.getElementById("SecGender");
  var inputAdress = document.getElementById("InAddress");
  var inputPassword = document.getElementById("InPassword");
  var inputId = document.getElementById("UserId");
  var saveButton = document.getElementById("savebtn");

  table.onclick = function (e) {
    var rowSelected = e.target.parentElement;
    for (var i = 0; i < rows.length; i++) {
      if (rows[i].className == "selected") {
        if (rows[i].className == rowSelected.className) {
          rows[i].classList.remove("selected");
          inputName.value = "";
          inputPhone.value = "";
          inputGender.value = "";
          inputAdress.value = "";
          inputPassword.value = "";
          inputId.value = "";
          saveButton.style.display = "block";
          return;
        }
        rows[i].classList.remove("selected");
      }
    }
    // hide the save button
    saveButton.style.display = "none";

    rowSelected.className = "selected";

    Array.from(rowSelected.cells, (el) => {
      switch (el.id) {
        case "UName":
          inputName.value = el.innerText;
          return;
        case "UPhone": {
          inputPhone.value = el.innerText;
          return;
        }
        case "UAddress": {
          inputAdress.value = el.innerText;
          return;
        }
        case "UPassword": {
          inputPassword.value = el.innerText;
          return;
        }
        case "UGender": {
          inputGender.value =
            el.innerText == "Female" ? 0 : el.innerText == "Male" ? 1 : 2;
          return;
        }
        case "UserId": {
          inputId.value = el.innerText;
        }
        default:
          break;
      }
    });
  };
}

function role_highlight_row() {
  var table = document.getElementById("roleTable");
  var rows = table.getElementsByTagName("tr");
  var inputRole = document.getElementById("InRole");
  var inputId = document.getElementById("RoleId");
  var saveButton = document.getElementById("savebtn");

  table.onclick = function (e) {
    var rowSelected = e.target.parentElement;
    for (var i = 0; i < rows.length; i++) {
      if (rows[i].className == "selected") {
        if (rows[i].className == rowSelected.className) {
          rows[i].classList.remove("selected");
          inputRole.value = "";
          inputId.value = "";
          saveButton.style.display = "block";
          return;
        }
        rows[i].classList.remove("selected");
      }
    }
    // hide the save button
    saveButton.style.display = "none";

    rowSelected.className = "selected";

    Array.from(rowSelected.cells, (el) => {
      switch (el.id) {
        case "RName":
          inputRole.value = el.innerText;
          return;
        case "RoleId": {
          inputId.value = el.innerText;
          return;
        }

        default:
          break;
      }
    });
  };
}

var run = () => {
  var pageIdentifier = document.getElementById("page");
  let identifier = pageIdentifier.dataset.pageIdentifier;

  if (identifier == "category") {
    cat_highlight_row();
  }
  if (identifier == "room") {
    room_highlight_row();
  }
  if (identifier == "user") {
    user_highlight_row();
  }
  if (identifier == "role") {
    role_highlight_row();
  }
};

run();
