var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        //make ajax request to get the data
        "ajax": {
            "url": "/Admin/Manga/GetAll"
        },
        //pass columns after receiving ajax data
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "genre.name", "width": "15%" },
            { "data": "author.name", "width": "15%" },
            { "data": "status", "width": "15%" },
            { "data": "chapters", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Manga/Upsert?id=${data}"
                       class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       <a 
                       class="btn btn-danger mx-2"> <i class="bi bi-trash"></i> Remove</a>
                    </div>
                        `
                },
                "width": "15%"
            }
            //{ "data": "updated", "width": "15%" },
            //{ "data": "rating", "width": "15%" },
            //{ "data": "views", "width": "15%" }
        ]
    });
}