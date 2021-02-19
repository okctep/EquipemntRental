$.cookie("rentalList", null, { path: '/' });

$(".addToCardBtn").click(function () {

    var equipment = {
        Name: $(this).closest('.card-body').children(".card-title").html(),
        Type: $(this).closest('.card-body').children(".equipment-type").html(),
        RentalDays: $(this).closest('.input-group').children('.rental-days').val()
    };

    addToRentalList(equipment);
});

$("#shopBasket").click(function () {

    if ($.cookie('rentalList') != "null") {
        var rentedEquipemnt = JSON.parse($.cookie('rentalList'));
        var table = makeTable(rentedEquipemnt);

        $("#rentedEquipemntListModal .modal-body").html(table);
        $('#rentedEquipemntListModal').modal('show');
    }
});

$("#generateInvoice").click(function () {
    $.ajax({
        url: "/Home/GetInvoice",
        type: "POST",
        dataType: "json",
        data: { constructionEquipemnt: JSON.parse($.cookie("rentalList")) },
        success: function (data) {
            download("Invoice",data);
        },
    });
});

function download(filename, text) {
    const a = document.createElement("a");
    a.href = URL.createObjectURL(new Blob([JSON.stringify(text, null, 2).replace(/[\[{,}\]"]+/g, "").trim()], {
        type: "text/plain"
    }));
    a.setAttribute("download", filename +".txt");
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}


function makeTable(mydata) {
    var table = $('<table class="table table-bordered" border=1>');
    var tblHeader = "<tr>";
    for (var k in mydata[0]) tblHeader += "<th>" + k.toUpperCase() + "</th>";
    tblHeader += "</tr>";
    $(tblHeader).appendTo(table);
    $.each(mydata, function (index, value) {
        var TableRow = "<tr>";
        $.each(value, function (key, val) {
            TableRow += "<td>" + val + "</td>";
        });
        TableRow += "</tr>";
        $(table).append(TableRow);
    });
    return ($(table));
};

function addToRentalList(equipment) {

    var rentedEquipmentList = new Array();

    if ($.cookie('rentalList') !== "null") {
        rentedEquipmentList = JSON.parse($.cookie('rentalList'));
        rentedEquipmentList.push(equipment);
    }
    else {
        rentedEquipmentList.push(equipment);
    }
    $.cookie('rentalList', JSON.stringify(rentedEquipmentList), { expires: 1 });
    addBasketEquipemntCount(rentedEquipmentList.length);
};

function addBasketEquipemntCount(equipemntCount) {
    $(".dot").html(equipemntCount);
    $(".dot").removeAttr('hidden');
}