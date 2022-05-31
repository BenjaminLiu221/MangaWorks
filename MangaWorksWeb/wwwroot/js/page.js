var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        //make ajax request to get the data
        "ajax": {
            "url": "/Admin/Page/GetAll"
        },
        //pass columns after receiving ajax data
        "columns": [
            { "data": "chapter.id", "width": "15%" },
            { "data": "pageNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Page/Upsert?id=${data}"
                       class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       <a onClick=Delete('/Admin/Page/Delete/${data}')
                       class="btn btn-danger mx-2"> <i class="bi bi-trash"></i> Remove</a>
                    </div>
                        `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}