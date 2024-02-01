var dataTable;
$(document).ready(function () {
    LoadData();
});
function LoadData() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Product/GetAll' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "20%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a class="btn btn-primary mx-2" href="/admin/product/upsert?id=${data}">Edit</a>
                    <a class="btn btn-danger mx-2" onClick=Delete('/admin/product/delete?id=${data}')>Delete</a>
                    </div>`;
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}


