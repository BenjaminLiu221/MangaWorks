$(document).ready(function () {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Chapter/GetAll"
        },
        "columns": [
            { "data": "manga.title", "width": "15%" },
            { "data": "chapterNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Chapter/Upsert?id=${data}"
                       class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       <a onClick=Delete('/Admin/Chapter/Delete/${data}')
                       class="btn btn-danger mx-2"> <i class="bi bi-trash"></i> Remove</a>
                    </div>
                        `
                },
                "width": "15%"
            }
        ],
        initComplete: function () {
            this.api()
                .columns()
                .every(function () {
                    var column = this;
                    var select = $('<select><option value="">Display All</option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());

                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });

                    column
                        .data()
                        .unique()
                        .sort(function (a, b) { return a - b })
                        .each(function (d) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                });
        },
    });
});

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