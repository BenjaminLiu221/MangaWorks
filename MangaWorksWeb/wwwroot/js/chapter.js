var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        //make ajax request to get the data
        "ajax": {
            "url": "/Admin/Manga/ChapterGetAll"
        },
        //pass columns after receiving ajax data
        "columns": [
            { "data": "Manga.Id", "width": "15%" },
            { "data": "ChapterNumber", "width": "15%" },
        ]
    });
}

//function Delete(url) {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    if (data.success) {
//                        dataTable.ajax.reload();
//                        toastr.success(data.message);
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            })
//        }
//    })
//}