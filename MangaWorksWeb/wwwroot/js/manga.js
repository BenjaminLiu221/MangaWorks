var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        //make ajax request to get the data
        "ajax": {
            "url":"/Admin/Manga/GetAll"
        },
        //pass columns after receiving ajax data
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "genre.name", "width": "15%" },
            { "data": "author.name", "width": "15%" },
            { "data": "status", "width": "15%" },
            { "data": "chapters", "width": "15%" },
            //{ "data": "updated", "width": "15%" },
            //{ "data": "rating", "width": "15%" },
            //{ "data": "views", "width": "15%" }
        ]
    });
}